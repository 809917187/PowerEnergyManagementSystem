using Dapper;
using IAMS.Models.StationSystem;
using IAMS.MQTT.Model;
using IAMS.Service;
using MySql.Data.MySqlClient;
using System.Text.Json;

namespace IAMS.MQTT {
    public class MQTTHelper {
        private static string _connectionString;
        public static void SetConnectionString(string connectionString) {
            _connectionString = connectionString;
        }
        public static bool SaveMqttPeriodDataToDB() {
            try {
                string json = MQTTHelper.GetPeriodData();
                var rootObject = JsonSerializer.Deserialize<DeviceDataFromMqtt>(json);
                if (rootObject != null) {
                    //DateTime UploadTime = DateTimeOffset.FromUnixTimeSeconds(rootObject.timeStamp).DateTime;
                    DateTime UploadTime = DateTime.Now;
                    //003储能堆控
                    MQTTHelper.SaveEnergyStorageStackControlInfo(MQTTHelper.ConvertMqttDataToEnergyStorageStackControlInfos(rootObject.devData.FindAll(s => s.devType == "3").ToList(), UploadTime));
                    //001关口表模型
                    MQTTHelper.SaveGatewayTableModelInfo(MQTTHelper.ConvertMqttDataToGatewayTableModelInfos(rootObject.devData.FindAll(s => s.devType == "1").ToList(), UploadTime));
                    //005pcs
                    MQTTHelper.SavePcsInfo(MQTTHelper.ConvertMqttDataToPcsInfos(rootObject.devData.FindAll(s => s.devType == "5").ToList(), UploadTime));
                }


            } catch (Exception e) {
                return false;
            }

            return true;
        }

        public static bool SaveRootDataToDBInfo() {
            string json = MQTTHelper.GetRootData();
            MQTTHelper.SaveRootInfo(json);
            return true;
        }

        public static List<Structure> FindStructuresBydevTypeAndmenuTree(Structure root, int devType, int menuTree) {
            var result = new List<Structure>();

            if (root == null) return result;

            // 检查当前节点
            if (root.devType == devType && root.menuTree == menuTree) {
                result.Add(root);
            }

            // 遍历子节点
            if (root.child != null) {
                foreach (var childNode in root.child) {
                    result.AddRange(FindStructuresBydevTypeAndmenuTree(childNode, devType, menuTree));
                }
            }

            return result;
        }

        public static RootDataFromMqtt ConvertRootInfoToObject(string json) {
            RootDataFromMqtt root = new RootDataFromMqtt();
            try {
                root = JsonSerializer.Deserialize<RootDataFromMqtt>(json);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            return root;

        }

        /*
         存储到energy_storage_cabinet_array表
         */
        public static bool SaveRootInfo(string json) {
            if (string.IsNullOrWhiteSpace(json)) {
                return false;
            }

            try {
                RootDataFromMqtt root = MQTTHelper.ConvertRootInfoToObject(json);
                using (var connection = new MySqlConnection(_connectionString)) {
                    connection.Open();
                    const string sql = "INSERT INTO energy_storage_cabinet_array (name,json_structure) VALUES (@name,@json_structure)";
                    using (MySqlCommand cmd = new MySqlCommand(sql, connection)) {
                        cmd.Parameters.AddWithValue("@name", root.structure.name);
                        cmd.Parameters.AddWithValue("@json_structure", json);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public static bool SavePcsInfo(List<PCSInfo> infos) {
            if (infos == null || infos.Count == 0) {
                return false;
            }
            try {
                using (var connection = new MySqlConnection(_connectionString)) {
                    connection.Open();
                    const string insertQuery = @"
                        INSERT INTO device_pcs_info (
                            upload_time, dev_type, dev_name, dev_id, sn, 
                            is_enabled, is_online, total_fault, total_alarm, 
                            hardware_overcurrent_phase_a, hardware_overcurrent_phase_b, hardware_overcurrent_phase_c, 
                            hardware_overcurrent_phase_n, unit_dc_voltage, switch_power_under_voltage, 
                            igbt_fault_phase_a, igbt_fault_phase_b, igbt_fault_phase_c, igbt_fault_phase_n, 
                            over_temperature_fault, output_overcurrent_phase_a, output_short_circuit_phase_a, 
                            output_overcurrent_phase_b, output_short_circuit_phase_b, output_overcurrent_phase_c, 
                            output_short_circuit_phase_c, output_overcurrent_phase_n, output_short_circuit_phase_n, 
                            ac_over_voltage, ac_under_voltage, ac_over_frequency, ac_under_frequency, 
                            voltage_thdu_exceed, system_phase_loss, system_phase_sequence_error, dc_polarity_reverse, 
                            dc_bus_under_voltage, dc_bus_over_voltage, system_over_frequency, system_under_frequency, 
                            dc_charging_overcurrent, dc_discharging_overcurrent, island_protection, reserved_fault_and_status, 
                            voltage_phase_a, voltage_phase_b, voltage_phase_c, current_phase_a, current_phase_b, 
                            current_phase_c, grid_frequency, active_power_phase_a, active_power_phase_b, active_power_phase_c, 
                            total_active_power, reactive_power_phase_a, reactive_power_phase_b, reactive_power_phase_c, 
                            total_reactive_power, apparent_power_phase_a, apparent_power_phase_b, apparent_power_phase_c, 
                            total_apparent_power, power_factor_phase_a, power_factor_phase_b, power_factor_phase_c, 
                            total_power_factor, pcs_entry_power, pcs_entry_voltage, pcs_entry_current, 
                            daily_charge_amount, daily_discharge_amount, ac_cumulative_charge_amount, 
                            ac_cumulative_discharge_amount, dc_cumulative_charge_amount, dc_cumulative_discharge_amount, 
                            max_allowable_charge_current, max_allowable_discharge_current, max_allowable_charge_power, 
                            max_allowable_discharge_power, igbt_temperature, pcs_fault_word_1, pcs_fault_word_2, 
                            pcs_fault_word_3, pcs_fault_word_4, pcs_fault_word_5, pcs_fault_word_6, system_clock_seconds, 
                            system_clock_minutes, system_clock_hours, system_clock_day, system_clock_month, system_clock_year, 
                            reserved_2, remote_local_setting, operating_mode_setting, grid_connection_disconnection_setting, 
                            device_power_on, device_power_off, constant_power_reactive_power_setting, power_factor_control, 
                            reserved_remote_adjustment_and_control
                        )
                        VALUES (
                            @UploadTime, @DevType, @DevName, @DevId, @Sn, 
                            @IsEnabled, @IsOnline, @TotalFault, @TotalAlarm, 
                            @HardwareOvercurrentPhaseA, @HardwareOvercurrentPhaseB, @HardwareOvercurrentPhaseC, 
                            @HardwareOvercurrentPhaseN, @UnitDcVoltage, @SwitchPowerUnderVoltage, 
                            @IgbtFaultPhaseA, @IgbtFaultPhaseB, @IgbtFaultPhaseC, @IgbtFaultPhaseN, 
                            @OverTemperatureFault, @OutputOvercurrentPhaseA, @OutputShortCircuitPhaseA, 
                            @OutputOvercurrentPhaseB, @OutputShortCircuitPhaseB, @OutputOvercurrentPhaseC, 
                            @OutputShortCircuitPhaseC, @OutputOvercurrentPhaseN, @OutputShortCircuitPhaseN, 
                            @AcOverVoltage, @AcUnderVoltage, @AcOverFrequency, @AcUnderFrequency, 
                            @VoltageThduExceed, @SystemPhaseLoss, @SystemPhaseSequenceError, @DcPolarityReverse, 
                            @DcBusUnderVoltage, @DcBusOverVoltage, @SystemOverFrequency, @SystemUnderFrequency, 
                            @DcChargingOvercurrent, @DcDischargingOvercurrent, @IslandProtection, @ReservedFaultAndStatus, 
                            @VoltagePhaseA, @VoltagePhaseB, @VoltagePhaseC, @CurrentPhaseA, @CurrentPhaseB, 
                            @CurrentPhaseC, @GridFrequency, @ActivePowerPhaseA, @ActivePowerPhaseB, @ActivePowerPhaseC, 
                            @TotalActivePower, @ReactivePowerPhaseA, @ReactivePowerPhaseB, @ReactivePowerPhaseC, 
                            @TotalReactivePower, @ApparentPowerPhaseA, @ApparentPowerPhaseB, @ApparentPowerPhaseC, 
                            @TotalApparentPower, @PowerFactorPhaseA, @PowerFactorPhaseB, @PowerFactorPhaseC, 
                            @TotalPowerFactor, @PcsEntryPower, @PcsEntryVoltage, @PcsEntryCurrent, 
                            @DailyChargeAmount, @DailyDischargeAmount, @AcCumulativeChargeAmount, 
                            @AcCumulativeDischargeAmount, @DcCumulativeChargeAmount, @DcCumulativeDischargeAmount, 
                            @MaxAllowableChargeCurrent, @MaxAllowableDischargeCurrent, @MaxAllowableChargePower, 
                            @MaxAllowableDischargePower, @IgbtTemperature, @PcsFaultWord1, @PcsFaultWord2, 
                            @PcsFaultWord3, @PcsFaultWord4, @PcsFaultWord5, @PcsFaultWord6, @SystemClockSeconds, 
                            @SystemClockMinutes, @SystemClockHours, @SystemClockDay, @SystemClockMonth, @SystemClockYear, 
                            @Reserved2, @RemoteLocalSetting, @OperatingModeSetting, @GridConnectionDisconnectionSetting, 
                            @DevicePowerOn, @DevicePowerOff, @ConstantPowerReactivePowerSetting, @PowerFactorControl, 
                            @ReservedRemoteAdjustmentAndControl
                        );
                    ";
                    using (var transaction = connection.BeginTransaction()) {
                        connection.Execute(insertQuery, infos, transaction: transaction);
                        transaction.Commit();
                    }
                    return true;
                }
            } catch (Exception ex) {
                return false;
            }
        }
        public static bool SaveGatewayTableModelInfo(List<GatewayTableModelInfo> infos) {
            if (infos == null || infos.Count == 0) {
                return false;
            }
            try {
                using (var connection = new MySqlConnection(_connectionString)) {
                    connection.Open();
                    const string insertQuery = @"
                        INSERT INTO device_gateway_table_model (
                            upload_time, dev_type, dev_name, dev_id, sn, 
                            is_enabled, is_online,
                            voltage_phase_a, voltage_phase_b, voltage_phase_c,
                            voltage_line_ab, voltage_line_bc, voltage_line_ca,
                            current_phase_a, current_phase_b, current_phase_c,
                            active_power_phase_a, active_power_phase_b, active_power_phase_c,
                            total_active_power, reactive_power_phase_a, reactive_power_phase_b, reactive_power_phase_c,
                            total_reactive_power, apparent_power_phase_a, apparent_power_phase_b, apparent_power_phase_c,
                            total_apparent_power, power_factor_phase_a, power_factor_phase_b, power_factor_phase_c,
                            total_power_factor, grid_frequency, voltage_transformation_ratio, current_transformation_ratio,
                            forward_active_energy, reverse_active_energy, forward_reactive_energy, reverse_reactive_energy,
                            peak_forward_active_energy, peak_reverse_active_energy,
                            flat_forward_active_energy, flat_reverse_active_energy,
                            normal_forward_active_energy, normal_reverse_active_energy,
                            valley_forward_active_energy, valley_reverse_active_energy,
                            current_month_max_forward_active_demand, current_month_max_reverse_active_demand
                        )
                        VALUES (
                            @UploadTime, @DevType, @DevName, @DevId, @Sn, 
                            @IsEnabled, @IsOnline,
                            @VoltagePhaseA, @VoltagePhaseB, @VoltagePhaseC,
                            @VoltageLineAB, @VoltageLineBC, @VoltageLineCA,
                            @CurrentPhaseA, @CurrentPhaseB, @CurrentPhaseC,
                            @ActivePowerPhaseA, @ActivePowerPhaseB, @ActivePowerPhaseC,
                            @TotalActivePower, @ReactivePowerPhaseA, @ReactivePowerPhaseB, @ReactivePowerPhaseC,
                            @TotalReactivePower, @ApparentPowerPhaseA, @ApparentPowerPhaseB, @ApparentPowerPhaseC,
                            @TotalApparentPower, @PowerFactorPhaseA, @PowerFactorPhaseB, @PowerFactorPhaseC,
                            @TotalPowerFactor, @GridFrequency, @VoltageTransformationRatio, @CurrentTransformationRatio,
                            @ForwardActiveEnergy, @ReverseActiveEnergy, @ForwardReactiveEnergy, @ReverseReactiveEnergy,
                            @PeakForwardActiveEnergy, @PeakReverseActiveEnergy,
                            @FlatForwardActiveEnergy, @FlatReverseActiveEnergy,
                            @NormalForwardActiveEnergy, @NormalReverseActiveEnergy,
                            @ValleyForwardActiveEnergy, @ValleyReverseActiveEnergy,
                            @CurrentMonthMaxForwardActiveDemand, @CurrentMonthMaxReverseActiveDemand
                        );
                    ";
                    using (var transaction = connection.BeginTransaction()) {
                        connection.Execute(insertQuery, infos, transaction: transaction);
                        transaction.Commit();
                    }
                    return true;
                }
            } catch (Exception ex) {
                return false;
            }

        }
        public static bool SaveEnergyStorageStackControlInfo(List<EnergyStorageStackControlInfo> energyStorageStackControlInfos) {
            if (energyStorageStackControlInfos == null || energyStorageStackControlInfos.Count == 0) {
                return false;
            }

            try {
                using (var connection = new MySqlConnection(_connectionString)) {
                    connection.Open();
                    const string insertQuery = @"
    INSERT INTO device_energy_storage_stack_control_info (
    upload_time, dev_type, dev_name, dev_id, sn, 
    device_enabled, device_online, total_voltage, total_current, state_of_charge, state_of_health,
    state_of_energy, rated_total_voltage, rated_capacity, remaining_capacity, rated_energy,
    remaining_energy, total_number_of_slave_units_bmu, online_number_of_slave_units_bmu,
    total_number_of_batteries, online_number_of_batteries, total_number_of_temperature_sensors,
    online_number_of_temperature_sensors, maximum_allowed_discharge_current,
    maximum_allowed_discharge_power, maximum_allowed_charge_current, maximum_allowed_charge_power,
    positive_insulation_resistance, negative_insulation_resistance, average_cell_voltage,
    maximum_voltage_difference_between_cells, highest_cell_voltage,
    highest_cell_voltage_slave_unit_number, highest_cell_voltage_serial_number, lowest_cell_voltage,
    lowest_cell_voltage_slave_unit_number, lowest_cell_voltage_serial_number, average_cell_temperature,
    maximum_temperature_difference, highest_cell_temperature, highest_cell_temperature_slave_unit_number,
    highest_cell_temperature_serial_number, lowest_cell_temperature, lowest_cell_temperature_slave_unit_number,
    lowest_cell_temperature_serial_number, daily_charge_capacity, daily_charge_energy,
    daily_discharge_capacity, daily_discharge_energy, daily_charge_time, daily_discharge_time,
    cumulative_charge_capacity, cumulative_charge_energy, cumulative_discharge_capacity,
    cumulative_discharge_energy
)
VALUES (
    @UploadTime, @DevType, @DevName, @DevId, @Sn, 
    @DeviceEnabled, @DeviceOnline, @TotalVoltage, @TotalCurrent, @StateOfCharge, @StateOfHealth,
    @StateOfEnergy, @RatedTotalVoltage, @RatedCapacity, @RemainingCapacity, @RatedEnergy,
    @RemainingEnergy, @TotalNumberOfSlaveUnitsBMU, @OnlineNumberOfSlaveUnitsBMU,
    @TotalNumberOfBatteries, @OnlineNumberOfBatteries, @TotalNumberOfTemperatureSensors,
    @OnlineNumberOfTemperatureSensors, @MaximumAllowedDischargeCurrent,
    @MaximumAllowedDischargePower, @MaximumAllowedChargeCurrent, @MaximumAllowedChargePower,
    @PositiveInsulationResistance, @NegativeInsulationResistance, @AverageCellVoltage,
    @MaximumVoltageDifferenceBetweenCells, @HighestCellVoltage,
    @HighestCellVoltageSlaveUnitNumber, @HighestCellVoltageSerialNumber, @LowestCellVoltage,
    @LowestCellVoltageSlaveUnitNumber, @LowestCellVoltageSerialNumber, @AverageCellTemperature,
    @MaximumTemperatureDifference, @HighestCellTemperature, @HighestCellTemperatureSlaveUnitNumber,
    @HighestCellTemperatureSerialNumber, @LowestCellTemperature, @LowestCellTemperatureSlaveUnitNumber,
    @LowestCellTemperatureSerialNumber, @DailyChargeCapacity, @DailyChargeEnergy,
    @DailyDischargeCapacity, @DailyDischargeEnergy, @DailyChargeTime, @DailyDischargeTime,
    @CumulativeChargeCapacity, @CumulativeChargeEnergy, @CumulativeDischargeCapacity,
    @CumulativeDischargeEnergy
);";

                    using (var transaction = connection.BeginTransaction()) {
                        connection.Execute(insertQuery, energyStorageStackControlInfos, transaction: transaction);
                        transaction.Commit();
                    }
                    return true;
                }
            } catch (Exception ex) {
                return false;
            }

        }

        private static List<PCSInfo> ConvertMqttDataToPcsInfos(List<DataFromMqtt> deviceDatasFromMqtt, DateTime UploadTime) {
            List<PCSInfo> ret = new List<PCSInfo>();
            foreach (DataFromMqtt dataFromMqtt in deviceDatasFromMqtt) {

                string deviceName = dataFromMqtt.devName;
                bool GetBoolean(int index) => dataFromMqtt.data.ContainsKey(GetDataKey(index)) ? Convert.ToBoolean(dataFromMqtt.data[GetDataKey(index)]) : false; // 默认值为 false
                Random random = new Random();
                double GetDouble(int index) => dataFromMqtt.data.ContainsKey(GetDataKey(index)) ? Convert.ToDouble(dataFromMqtt.data[GetDataKey(index)]) + (random.NextDouble() * 50) : 0.0; // 默认值为 0.0
                string GetDataKey(int index) => MQTTHelper.GetDataKeyInMqtt(deviceName, index);
                try {

                    PCSInfo temp = new PCSInfo() {
                        UploadTime = UploadTime,
                        DevType = dataFromMqtt.devType,
                        DevName = deviceName,
                        DevId = dataFromMqtt.devId,
                        Sn = dataFromMqtt.sn,
                        IsEnabled = GetBoolean(0),
                        IsOnline = GetBoolean(1),
                        TotalFault = GetDouble(2),
                        TotalAlarm = GetDouble(3),
                        HardwareOvercurrentPhaseA = GetDouble(4),
                        HardwareOvercurrentPhaseB = GetDouble(5),
                        HardwareOvercurrentPhaseC = GetDouble(6),
                        HardwareOvercurrentPhaseN = GetDouble(7),
                        UnitDCVoltage = GetDouble(8),
                        SwitchPowerUnderVoltage = GetDouble(9),
                        IGBTFaultPhaseA = GetDouble(10),
                        IGBTFaultPhaseB = GetDouble(11),
                        IGBTFaultPhaseC = GetDouble(12),
                        IGBTFaultPhaseN = GetDouble(13),
                        OverTemperatureFault = GetDouble(14),
                        OutputOvercurrentPhaseA = GetDouble(15),
                        OutputShortCircuitPhaseA = GetDouble(16),
                        OutputOvercurrentPhaseB = GetDouble(17),
                        OutputShortCircuitPhaseB = GetDouble(18),
                        OutputOvercurrentPhaseC = GetDouble(19),
                        OutputShortCircuitPhaseC = GetDouble(20),
                        OutputOvercurrentPhaseN = GetDouble(21),
                        OutputShortCircuitPhaseN = GetDouble(22),
                        ACOverVoltage = GetDouble(23),
                        ACUnderVoltage = GetDouble(24),
                        ACOverFrequency = GetDouble(25),
                        ACUnderFrequency = GetDouble(26),
                        VoltageTHDUExceed = GetDouble(27),
                        SystemPhaseLoss = GetDouble(28),
                        SystemPhaseSequenceError = GetDouble(29),
                        DCPolarityReverse = GetDouble(30),
                        DCBusUnderVoltage = GetDouble(31),
                        DCBusOverVoltage = GetDouble(32),
                        SystemOverFrequency = GetDouble(33),
                        SystemUnderFrequency = GetDouble(34),
                        DCChargingOvercurrent = GetDouble(35),
                        DCDischargingOvercurrent = GetDouble(36),
                        IslandProtection = GetDouble(37),
                        ReservedFaultAndStatus = GetDouble(150),
                        VoltagePhaseA = GetDouble(151),
                        VoltagePhaseB = GetDouble(152),
                        VoltagePhaseC = GetDouble(153),
                        CurrentPhaseA = GetDouble(154),
                        CurrentPhaseB = GetDouble(155),
                        CurrentPhaseC = GetDouble(156),
                        GridFrequency = GetDouble(157),
                        ActivePowerPhaseA = GetDouble(158),
                        ActivePowerPhaseB = GetDouble(159),
                        ActivePowerPhaseC = GetDouble(160),
                        TotalActivePower = GetDouble(161),
                        ReactivePowerPhaseA = GetDouble(162),
                        ReactivePowerPhaseB = GetDouble(163),
                        ReactivePowerPhaseC = GetDouble(164),
                        TotalReactivePower = GetDouble(165),
                        ApparentPowerPhaseA = GetDouble(166),
                        ApparentPowerPhaseB = GetDouble(167),
                        ApparentPowerPhaseC = GetDouble(168),
                        TotalApparentPower = GetDouble(169),
                        PowerFactorPhaseA = GetDouble(170),
                        PowerFactorPhaseB = GetDouble(171),
                        PowerFactorPhaseC = GetDouble(172),
                        TotalPowerFactor = GetDouble(173),
                        PCSEntryPower = GetDouble(174),
                        PCSEntryVoltage = GetDouble(175),
                        PCSEntryCurrent = GetDouble(176),
                        DailyChargeAmount = GetDouble(177),
                        DailyDischargeAmount = GetDouble(178),
                        ACCumulativeChargeAmount = GetDouble(179),
                        ACCumulativeDischargeAmount = GetDouble(180),
                        DCCumulativeChargeAmount = GetDouble(181),
                        DCCumulativeDischargeAmount = GetDouble(182),
                        MaxAllowableChargeCurrent = GetDouble(183),
                        MaxAllowableDischargeCurrent = GetDouble(184),
                        MaxAllowableChargePower = GetDouble(185),
                        MaxAllowableDischargePower = GetDouble(186),
                        IGBTTemperature = GetDouble(187),
                        PCSFaultWord1 = GetDouble(188),
                        PCSFaultWord2 = GetDouble(189),
                        PCSFaultWord3 = GetDouble(190),
                        PCSFaultWord4 = GetDouble(191),
                        PCSFaultWord5 = GetDouble(192),
                        PCSFaultWord6 = GetDouble(193),
                        SystemClockSeconds = GetDouble(194),
                        SystemClockMinutes = GetDouble(195),
                        SystemClockHours = GetDouble(196),
                        SystemClockDay = GetDouble(197),
                        SystemClockMonth = GetDouble(198),
                        SystemClockYear = GetDouble(199),
                        Reserved2 = GetDouble(200 - 250),
                        RemoteLocalSetting = GetDouble(251),
                        OperatingModeSetting = GetDouble(252),
                        GridConnectionDisconnectionSetting = GetDouble(253),
                        DevicePowerOn = GetDouble(254),
                        DevicePowerOff = GetDouble(255),
                        ConstantPowerActivePowerSetting = GetDouble(256),
                        ConstantPowerReactivePowerSetting = GetDouble(257),
                        PowerFactorControl = GetDouble(258),
                        ReservedRemoteAdjustmentAndControl = GetDouble(259),
                    };

                    ret.Add(temp);
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }

            }
            return ret;
        }
        private static List<GatewayTableModelInfo> ConvertMqttDataToGatewayTableModelInfos(List<DataFromMqtt> deviceDatasFromMqtt, DateTime UploadTime) {
            List<GatewayTableModelInfo> ret = new List<GatewayTableModelInfo>();
            foreach (DataFromMqtt dataFromMqtt in deviceDatasFromMqtt) {

                string deviceName = dataFromMqtt.devName;
                bool GetBoolean(int index) => dataFromMqtt.data.ContainsKey(GetDataKey(index)) ? Convert.ToBoolean(dataFromMqtt.data[GetDataKey(index)]) : false; // 默认值为 false
                Random random = new Random();
                double GetDouble(int index) => dataFromMqtt.data.ContainsKey(GetDataKey(index)) ? Convert.ToDouble(dataFromMqtt.data[GetDataKey(index)]) + (random.NextDouble() * 50) : 0.0; // 默认值为 0.0
                string GetDataKey(int index) => MQTTHelper.GetDataKeyInMqtt(deviceName, index);
                try {

                    GatewayTableModelInfo temp = new GatewayTableModelInfo() {
                        UploadTime = UploadTime,
                        DevType = dataFromMqtt.devType,
                        DevName = deviceName,
                        DevId = dataFromMqtt.devId,
                        Sn = dataFromMqtt.sn,
                        IsEnabled = GetBoolean(0),
                        IsOnline = GetBoolean(1),
                        VoltagePhaseA = GetDouble(2),
                        VoltagePhaseB = GetDouble(3),
                        VoltagePhaseC = GetDouble(4),
                        VoltageLineAB = GetDouble(5),
                        VoltageLineBC = GetDouble(6),
                        VoltageLineCA = GetDouble(7),
                        CurrentPhaseA = GetDouble(8),
                        CurrentPhaseB = GetDouble(9),
                        CurrentPhaseC = GetDouble(10),
                        ActivePowerPhaseA = GetDouble(11),
                        ActivePowerPhaseB = GetDouble(12),
                        ActivePowerPhaseC = GetDouble(13),
                        TotalActivePower = GetDouble(14),
                        ReactivePowerPhaseA = GetDouble(15),
                        ReactivePowerPhaseB = GetDouble(16),
                        ReactivePowerPhaseC = GetDouble(17),
                        TotalReactivePower = GetDouble(18),
                        ApparentPowerPhaseA = GetDouble(19),
                        ApparentPowerPhaseB = GetDouble(20),
                        ApparentPowerPhaseC = GetDouble(21),
                        TotalApparentPower = GetDouble(22),
                        PowerFactorPhaseA = GetDouble(23),
                        PowerFactorPhaseB = GetDouble(24),
                        PowerFactorPhaseC = GetDouble(25),
                        TotalPowerFactor = GetDouble(26),
                        GridFrequency = GetDouble(27),
                        VoltageTransformationRatio = GetDouble(28),
                        CurrentTransformationRatio = GetDouble(29),
                        ForwardActiveEnergy = GetDouble(30),
                        ReverseActiveEnergy = GetDouble(31),
                        ForwardReactiveEnergy = GetDouble(32),
                        ReverseReactiveEnergy = GetDouble(33),
                        PeakForwardActiveEnergy = GetDouble(34),
                        PeakReverseActiveEnergy = GetDouble(35),
                        FlatForwardActiveEnergy = GetDouble(36),
                        FlatReverseActiveEnergy = GetDouble(37),
                        NormalForwardActiveEnergy = GetDouble(38),
                        NormalReverseActiveEnergy = GetDouble(39),
                        ValleyForwardActiveEnergy = GetDouble(40),
                        ValleyReverseActiveEnergy = GetDouble(41),
                        CurrentMonthMaxForwardActiveDemand = GetDouble(42),
                        CurrentMonthMaxReverseActiveDemand = GetDouble(43)
                    };

                    ret.Add(temp);
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }

            }
            return ret;
        }
        private static List<EnergyStorageStackControlInfo> ConvertMqttDataToEnergyStorageStackControlInfos(List<DataFromMqtt> deviceDatasFromMqtt, DateTime UploadTime) {
            List<EnergyStorageStackControlInfo> ret = new List<EnergyStorageStackControlInfo>();

            foreach (DataFromMqtt dataFromMqtt in deviceDatasFromMqtt) {

                string deviceName = dataFromMqtt.devName;
                bool GetBoolean(int index) => dataFromMqtt.data.ContainsKey(GetDataKey(index)) ? Convert.ToBoolean(dataFromMqtt.data[GetDataKey(index)]) : false; // 默认值为 false
                Random random = new Random();
                double GetDouble(int index) => dataFromMqtt.data.ContainsKey(GetDataKey(index)) ? Convert.ToDouble(dataFromMqtt.data[GetDataKey(index)]) + (random.NextDouble() * 50) : 0.0; // 默认值为 0.0
                string GetDataKey(int index) => MQTTHelper.GetDataKeyInMqtt(deviceName, index);
                try {

                    EnergyStorageStackControlInfo temp = new EnergyStorageStackControlInfo() {
                        UploadTime = UploadTime,
                        DevType = dataFromMqtt.devType,
                        DevName = deviceName,
                        DevId = dataFromMqtt.devId,
                        Sn = dataFromMqtt.sn,
                        DeviceEnabled = GetBoolean(0),
                        DeviceOnline = GetBoolean(1),
                        TotalVoltage = GetDouble(2),
                        TotalCurrent = GetDouble(3),
                        StateOfCharge = GetDouble(4),
                        StateOfHealth = GetDouble(5),
                        StateOfEnergy = GetDouble(6),
                        RatedTotalVoltage = GetDouble(7),
                        RatedCapacity = GetDouble(8),
                        RemainingCapacity = GetDouble(9),
                        RatedEnergy = GetDouble(10),
                        RemainingEnergy = GetDouble(11),
                        TotalNumberOfSlaveUnitsBMU = GetDouble(12),
                        OnlineNumberOfSlaveUnitsBMU = GetDouble(13),
                        TotalNumberOfBatteries = GetDouble(14),
                        OnlineNumberOfBatteries = GetDouble(15),
                        TotalNumberOfTemperatureSensors = GetDouble(16),
                        OnlineNumberOfTemperatureSensors = GetDouble(17),
                        MaximumAllowedDischargeCurrent = GetDouble(18),
                        MaximumAllowedDischargePower = GetDouble(19),
                        MaximumAllowedChargeCurrent = GetDouble(20),
                        MaximumAllowedChargePower = GetDouble(21),
                        PositiveInsulationResistance = GetDouble(22),
                        NegativeInsulationResistance = GetDouble(23),
                        AverageCellVoltage = GetDouble(24),
                        MaximumVoltageDifferenceBetweenCells = GetDouble(25),
                        HighestCellVoltage = GetDouble(26),
                        HighestCellVoltageSlaveUnitNumber = GetDouble(27),
                        HighestCellVoltageSerialNumber = GetDouble(28),
                        LowestCellVoltage = GetDouble(29),
                        LowestCellVoltageSlaveUnitNumber = GetDouble(30),
                        LowestCellVoltageSerialNumber = GetDouble(31),
                        AverageCellTemperature = GetDouble(32),
                        MaximumTemperatureDifference = GetDouble(33),
                        HighestCellTemperature = GetDouble(34),
                        HighestCellTemperatureSlaveUnitNumber = GetDouble(35),
                        HighestCellTemperatureSerialNumber = GetDouble(36),
                        LowestCellTemperature = GetDouble(37),
                        LowestCellTemperatureSlaveUnitNumber = GetDouble(38),
                        LowestCellTemperatureSerialNumber = GetDouble(39),
                        DailyChargeCapacity = GetDouble(40),
                        DailyChargeEnergy = GetDouble(41),
                        DailyDischargeCapacity = GetDouble(42),
                        DailyDischargeEnergy = GetDouble(43),
                        DailyChargeTime = GetDouble(44),
                        DailyDischargeTime = GetDouble(45),
                        CumulativeChargeCapacity = GetDouble(46),
                        CumulativeChargeEnergy = GetDouble(47),
                        CumulativeDischargeCapacity = GetDouble(48),
                        CumulativeDischargeEnergy = GetDouble(49),
                    };

                    ret.Add(temp);
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }

            }
            return ret;
        }

        private static string GetDataKeyInMqtt(string deviceName, int num) {
            return deviceName + "_" + num;
        }

        public static string GetPeriodData() {
            string filePath = Path.Combine(AppContext.BaseDirectory, "Assets", "JsonFile", "period.json");
            // 读取文件内容
            if (File.Exists(filePath)) {
                return File.ReadAllText(filePath).Replace("\r\n", "").Replace("\n", ""); ;
            } else {
                return String.Empty;
            }
        }

        public static string GetRootData() {
            string filePath = Path.Combine(AppContext.BaseDirectory, "Assets", "JsonFile", "root.json");
            // 读取文件内容
            if (File.Exists(filePath)) {
                return File.ReadAllText(filePath).Replace("\r\n", "").Replace("\n", ""); ;
            } else {
                return String.Empty;
            }
        }
    }


}
