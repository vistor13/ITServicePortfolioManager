export interface ServiceRequest {
  price: number;
  labourIntensity: number;
  incomeForProviders: number[];
  discount: number;
  indexGroup: number;
}

export interface ServiceGroupRequest {
  services: ServiceRequest[];
}

export interface TaskRequest {
  countProvider: number;
  totalHumanResource: number;
  services: ServiceGroupRequest[];
}
