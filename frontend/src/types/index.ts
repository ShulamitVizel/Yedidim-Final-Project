// TypeScript interfaces matching the backend models

export interface Call {
  callId: number;
  callTime: string;
  clientId: number;
  finalVolunteerId: number;
  callType: string;
  callLatitude: number;
  callLongitude: number;
  client?: Client | null;
  finalVolunteer?: Volunteer | null;
  volunteers: Volunteer[];
}

export interface Client {
  clientId: number;
  name: string;
  phoneNumber: string;
  calls: Call[];
}

export interface Volunteer {
  volunteerId: number;
  name: string;
  level: string;
  isAvailable: boolean;
  phoneNumber: string;
  volunteerLatitude: number;
  volunteerLongitude: number;
  calls: Call[];
  callsNavigation: Call[];
}

// DTOs for API requests
export interface CreateCallDto {
  callTime: string;
  clientId: number;
  finalVolunteerId: number | null;
  callType: string;
  callLatitude: number;
  callLongitude: number;
}

export interface UpdateCallDto {
  callId: number;
  callTime: string;
  clientId: number;
  finalVolunteerId: number | null;
  callType: string;
  callLatitude: number;
  callLongitude: number;
}

// API Response types
export interface EtaResponse {
  estimatedArrivalMinutes: number;
}

export interface AssignBestResponse {
  message: string;
} 