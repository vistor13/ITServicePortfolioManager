export interface ServiceResponse {
  price: number;
  labourIntensity: number;
  incomeForProviders: number[];
  discount: number;
}

export interface ServiceGroupResponse {
  serviceResponses: ServiceResponse[];
}

export interface TaskResponse {
  countProvider: number;
  totalHumanResource: number;
  services: ServiceGroupResponse[];
  algorithm:string;
  id:number;
}
