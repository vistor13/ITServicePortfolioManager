export interface DiscountDeltaResponse {
  discount: number;
  companyIncomeDeltaPercent: number;
  providersIncomeDeltaPercent: number;
  totalDeltaPercent: number;
}

export interface ResultWithDiscountResponse {
  discount: number;
  companyIncome: number;
  providersIncome: number[];
  portfolio: number[][];
}

export interface DiscountDeltaPopularServicesResponse {
  generalDeltas: DiscountDeltaResponse[];
  detailedDeltas: DiscountDeltaResponse[];
  bestResult?: ResultWithDiscountResponse;
  indexesServices?:number[];
}

export interface DiscountDeltaLowIncomeResponse {
  generalDeltas: DiscountDeltaResponse[];
  detailedDeltas: DiscountDeltaResponse[];
  bestResult?: ResultWithDiscountResponse;
  indexProvider?:number;
}
