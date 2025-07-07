import React, { useState, useEffect } from 'react';
import { Form, Button, Card, Alert, Row, Col, Spinner } from 'react-bootstrap';
import { useParams, useNavigate } from 'react-router-dom';
import { callApi, clientApi } from '../services/api';
import { CreateCallDto, UpdateCallDto, Client } from '../types';
import './CallForm.css';

const CallForm: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const isEditing = !!id;

  const [formData, setFormData] = useState<CreateCallDto>({
    callTime: new Date().toISOString().slice(0, 16),
    clientId: 0,
    finalVolunteerId: null,
    callType: '',
    callLatitude: 32.0853,
    callLongitude: 34.7818,
  });

  const [clients, setClients] = useState<Client[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  useEffect(() => {
    fetchClients();
    if (isEditing && id) {
      fetchCall(parseInt(id));
    }
  }, [id, isEditing]);

  const fetchClients = async () => {
    try {
      const clientsData = await clientApi.getAllClients();
      setClients(clientsData);
    } catch (err) {
      console.error('Error fetching clients:', err);
    }
  };

  const fetchCall = async (callId: number) => {
    try {
      setLoading(true);
      const call = await callApi.getCallById(callId);
      setFormData({
        callTime: new Date(call.callTime).toISOString().slice(0, 16),
        clientId: call.clientId,
        finalVolunteerId: call.finalVolunteerId === 0 ? null : call.finalVolunteerId,
        callType: call.callType,
        callLatitude: call.callLatitude,
        callLongitude: call.callLongitude,
      });
    } catch (err) {
      setError('Failed to fetch call');
      console.error('Error fetching call:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    setSuccess(null);

    try {
      if (isEditing && id) {
        const updateData: UpdateCallDto = {
          callId: parseInt(id),
          ...formData,
        };
        await callApi.updateCall(parseInt(id), updateData);
        setSuccess('Call updated successfully!');
      } else {
        await callApi.createCall(formData);
        setSuccess('Call created successfully!');
      }
      
      // Redirect after a short delay to show success message
      setTimeout(() => {
        navigate('/calls');
      }, 1500);
    } catch (err) {
      setError('Failed to save call. Please check your connection and try again.');
      console.error('Error saving call:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<any>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: name === 'callLatitude' || name === 'callLongitude' || name === 'clientId' 
        ? parseFloat(value) || 0 
        : value,
    }));
  };

  const handleLocationClick = () => {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          setFormData(prev => ({
            ...prev,
            callLatitude: position.coords.latitude,
            callLongitude: position.coords.longitude,
          }));
        },
        (error) => {
          setError('Unable to get your location. Please enter coordinates manually.');
        }
      );
    } else {
      setError('Geolocation is not supported by this browser.');
    }
  };

  if (loading && isEditing) {
    return (
      <div className="form-loading">
        <div className="loading-spinner">
          <Spinner animation="border" variant="primary" />
          <p className="mt-3">Loading call details...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="call-form-container">
      <div className="form-header">
        <h1 className="form-title">
          {isEditing ? 'Edit Call' : 'Create New Call'}
        </h1>
        <p className="form-subtitle">
          {isEditing ? 'Update call information' : 'Report an emergency or request assistance'}
        </p>
      </div>

      {error && (
        <Alert variant="danger" className="mb-4" onClose={() => setError(null)} dismissible>
          <Alert.Heading>Error</Alert.Heading>
          {error}
        </Alert>
      )}

      {success && (
        <Alert variant="success" className="mb-4">
          <Alert.Heading>Success!</Alert.Heading>
          {success}
        </Alert>
      )}

      <Card className="form-card">
        <Card.Body>
          <Form onSubmit={handleSubmit}>
            <Row>
              <Col lg={6}>
                <Form.Group className="mb-4">
                  <Form.Label className="form-label">
                    <span className="label-icon">🚨</span>
                    Call Type
                  </Form.Label>
                  <Form.Select
                    name="callType"
                    value={formData.callType}
                    onChange={handleInputChange}
                    required
                    className="form-control-modern"
                  >
                    <option value="">Select call type...</option>
                    <option value="Medical Emergency">Medical Emergency</option>
                    <option value="Roadside Assistance">Roadside Assistance</option>
                    <option value="Technical Support">Technical Support</option>
                    <option value="General Help">General Help</option>
                    <option value="Fire Emergency">Fire Emergency</option>
                    <option value="Security Issue">Security Issue</option>
                  </Form.Select>
                </Form.Group>
              </Col>
              
              <Col lg={6}>
                <Form.Group className="mb-4">
                  <Form.Label className="form-label">
                    <span className="label-icon">📅</span>
                    Call Time
                  </Form.Label>
                  <Form.Control
                    type="datetime-local"
                    name="callTime"
                    value={formData.callTime}
                    onChange={handleInputChange}
                    required
                    className="form-control-modern"
                  />
                </Form.Group>
              </Col>
            </Row>

            <Row>
              <Col lg={6}>
                <Form.Group className="mb-4">
                  <Form.Label className="form-label">
                    <span className="label-icon">👤</span>
                    Client
                  </Form.Label>
                  <Form.Select
                    name="clientId"
                    value={formData.clientId}
                    onChange={handleInputChange}
                    required
                    className="form-control-modern"
                  >
                    <option value={0}>Select a client...</option>
                    {clients.map((client) => (
                      <option key={client.clientId} value={client.clientId}>
                        {client.name} - {client.phoneNumber}
                      </option>
                    ))}
                  </Form.Select>
                  {clients.length === 0 && (
                    <small className="text-muted">
                      No clients available. <a href="/clients/new">Add a client first</a>
                    </small>
                  )}
                </Form.Group>
              </Col>
              
              <Col lg={6}>
                <Form.Group className="mb-4">
                  <Form.Label className="form-label">
                    <span className="label-icon">🤝</span>
                    Volunteer (Optional)
                  </Form.Label>
                  <Form.Control
                    type="number"
                    name="finalVolunteerId"
                    value={formData.finalVolunteerId || ''}
                    onChange={(e) => setFormData(prev => ({
                      ...prev,
                      finalVolunteerId: e.target.value ? parseInt(e.target.value) : null
                    }))}
                    placeholder="Leave empty for auto-assignment"
                    className="form-control-modern"
                  />
                  <small className="text-muted">
                    Leave empty to automatically assign the best available volunteer
                  </small>
                </Form.Group>
              </Col>
            </Row>

            <Row>
              <Col lg={12}>
                <div className="location-section">
                  <div className="location-header">
                    <h5 className="location-title">
                      <span className="label-icon">📍</span>
                      Location
                    </h5>
                    <Button
                      type="button"
                      variant="outline-primary"
                      size="sm"
                      onClick={handleLocationClick}
                      className="location-button"
                    >
                      📍 Use My Location
                    </Button>
                  </div>
                  
                  <Row>
                    <Col md={6}>
                      <Form.Group className="mb-3">
                        <Form.Label>Latitude</Form.Label>
                        <Form.Control
                          type="number"
                          step="0.0001"
                          name="callLatitude"
                          value={formData.callLatitude}
                          onChange={handleInputChange}
                          required
                          className="form-control-modern"
                        />
                      </Form.Group>
                    </Col>
                    <Col md={6}>
                      <Form.Group className="mb-3">
                        <Form.Label>Longitude</Form.Label>
                        <Form.Control
                          type="number"
                          step="0.0001"
                          name="callLongitude"
                          value={formData.callLongitude}
                          onChange={handleInputChange}
                          required
                          className="form-control-modern"
                        />
                      </Form.Group>
                    </Col>
                  </Row>
                </div>
              </Col>
            </Row>

            <div className="form-actions">
              <Button 
                type="submit" 
                variant="primary" 
                disabled={loading}
                className="submit-button"
              >
                {loading ? (
                  <>
                    <Spinner animation="border" size="sm" className="me-2" />
                    {isEditing ? 'Updating...' : 'Creating...'}
                  </>
                ) : (
                  <>
                    <span className="button-icon">
                      {isEditing ? '💾' : '🚀'}
                    </span>
                    {isEditing ? 'Update Call' : 'Create Call'}
                  </>
                )}
              </Button>
              
              <Button 
                type="button" 
                variant="outline-secondary" 
                onClick={() => navigate('/calls')}
                className="cancel-button"
              >
                <span className="button-icon">❌</span>
                Cancel
              </Button>
            </div>
          </Form>
        </Card.Body>
      </Card>
    </div>
  );
};

export default CallForm; 