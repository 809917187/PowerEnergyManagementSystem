﻿using IAMS.Models.PriceTemplate;

namespace IAMS.Service {
    public interface ITemplateService {
        public bool AddTemplate(PriceTemplateInfo priceTemplate);
        public List<PriceTemplateInfo> GetAllPriceTemplateInfos();
        public bool DeleteTemplateInfo(int templateId);

        public PriceTemplateInfo GetPriceTemplateInfoById(int id);
        public List<TimeFrameInfo> GetTimeFrameInfoById(int templateId);
        public Dictionary<int, decimal> GetTimeFrame2BuyPrice(int templateId);
        public Dictionary<int, decimal> GetTimeFrame2SalePrice(int templateId);
        public bool UpdatePriceTemplate(PriceTemplateInfo priceTemplate);
    }
}