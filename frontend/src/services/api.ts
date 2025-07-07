import axios from 'axios';
import { Call, Client, Volunteer, CreateCallDto, UpdateCallDto, EtaResponse, AssignBestResponse } from '../types';

// API base URL - matches your backend
const API_BASE_URL = 'https://localhost:7146/api';

// Create axios instance with base configuration
const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Call API endpoints
export const callApi = {
  // Get all calls
  getAllCalls: async (): Promise<Call[]> => {
    const response = await api.get('/Call');
    return response.data;
  },

  // Get call by ID
  getCallById: async (id: number): Promise<Call> => {
    const response = await api.get(`/Call/${id}`);
    return response.data;
  },

  // Create new call
  createCall: async (callData: CreateCallDto): Promise<Call> => {
    const response = await api.post('/Call', callData);
    return response.data;
  },

  // Update call
  updateCall: async (id: number, callData: UpdateCallDto): Promise<void> => {
    await api.put(`/Call/${id}`, callData);
  },

  // Delete call
  deleteCall: async (id: number): Promise<void> => {
    await api.delete(`/Call/${id}`);
  },

  // Assign volunteer to call
  assignVolunteer: async (callId: number, volunteerId: number): Promise<void> => {
    await api.post(`/Call/${callId}/volunteers/${volunteerId}`);
  },

  // Assign best volunteer automatically
  assignBestVolunteer: async (callId: number): Promise<AssignBestResponse> => {
    const response = await api.post(`/Call/${callId}/assign-best`);
    return response.data;
  },

  // Get matching volunteers for a call
  getMatchingVolunteers: async (callId: number): Promise<Volunteer[]> => {
    const response = await api.get(`/Call/${callId}/matching-volunteers`);
    return response.data;
  },

  // Get ETA for volunteer to reach call
  getEta: async (callId: number, volunteerId: number): Promise<EtaResponse> => {
    const response = await api.get(`/Call/${callId}/eta?volunteerId=${volunteerId}`);
    return response.data;
  },
};

// Client API endpoints
export const clientApi = {
  // Get all clients
  getAllClients: async (): Promise<Client[]> => {
    const response = await api.get('/Client');
    return response.data;
  },

  // Get client by ID
  getClientById: async (id: number): Promise<Client> => {
    const response = await api.get(`/Client/${id}`);
    return response.data;
  },

  // Create new client
  createClient: async (clientData: Omit<Client, 'clientId' | 'calls'>): Promise<Client> => {
    const response = await api.post('/Client', clientData);
    return response.data;
  },

  // Update client
  updateClient: async (id: number, clientData: Omit<Client, 'clientId' | 'calls'>): Promise<void> => {
    await api.put(`/Client/${id}`, clientData);
  },

  // Delete client
  deleteClient: async (id: number): Promise<void> => {
    await api.delete(`/Client/${id}`);
  },
};

// Volunteer API endpoints
export const volunteerApi = {
  // Get all volunteers
  getAllVolunteers: async (): Promise<Volunteer[]> => {
    const response = await api.get('/Volunteer');
    return response.data;
  },

  // Get volunteer by ID
  getVolunteerById: async (id: number): Promise<Volunteer> => {
    const response = await api.get(`/Volunteer/${id}`);
    return response.data;
  },

  // Create new volunteer
  createVolunteer: async (volunteerData: Omit<Volunteer, 'volunteerId' | 'calls' | 'callsNavigation'>): Promise<Volunteer> => {
    const response = await api.post('/Volunteer', volunteerData);
    return response.data;
  },

  // Update volunteer
  updateVolunteer: async (id: number, volunteerData: Omit<Volunteer, 'volunteerId' | 'calls' | 'callsNavigation'>): Promise<void> => {
    await api.put(`/Volunteer/${id}`, volunteerData);
  },

  // Delete volunteer
  deleteVolunteer: async (id: number): Promise<void> => {
    await api.delete(`/Volunteer/${id}`);
  },
};

export default api; 