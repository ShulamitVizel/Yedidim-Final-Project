import React, { useState, useEffect } from 'react';
import { Form, Button, Card, Alert, Row, Col, Spinner } from 'react-bootstrap';
import { useParams, useNavigate } from 'react-router-dom';
import { clientApi } from '../services/api';
import { Client } from '../types';
import './ClientForm.css';

const ClientForm: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const isEditing = !!id;

  const [formData, setFormData] = useState<Omit<Client, 'clientId' | 'calls'>>({
    name: '',
    phoneNumber: '',
  });

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  useEffect(() => {
    if (isEditing && id) {
      fetchClient(parseInt(id));
    }
  }, [id, isEditing]);

  const fetchClient = async (clientId: number) => {
    try {
      setLoading(true);
      const client = await clientApi.getClientById(clientId);
      setFormData({
        name: client.name,
        phoneNumber: client.phoneNumber,
      });
    } catch (err) {
      setError('Failed to fetch client');
      console.error('Error fetching client:', err);
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
        await clientApi.updateClient(parseInt(id), formData);
        setSuccess('Client updated successfully!');
      } else {
        await clientApi.createClient(formData);
        setSuccess('Client created successfully!');
      }
      
      // Redirect after a short delay to show success message
      setTimeout(() => {
        navigate('/clients');
      }, 1500);
    } catch (err) {
      setError('Failed to save client. Please check your connection and try again.');
      console.error('Error saving client:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value,
    }));
  };

  if (loading && isEditing) {
    return (
      <div className="form-loading">
        <div className="loading-spinner">
          <Spinner animation="border" variant="primary" />
          <p className="mt-3">Loading client details...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="client-form-container">
      <div className="form-header">
        <h1 className="form-title">
          {isEditing ? 'Edit Client' : 'Add New Client'}
        </h1>
        <p className="form-subtitle">
          {isEditing ? 'Update client information' : 'Register a new client in the system'}
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
                    <span className="label-icon">👤</span>
                    Client Name
                  </Form.Label>
                  <Form.Control
                    type="text"
                    name="name"
                    value={formData.name}
                    onChange={handleInputChange}
                    required
                    className="form-control-modern"
                    placeholder="Enter client's full name"
                  />
                </Form.Group>
              </Col>
              
              <Col lg={6}>
                <Form.Group className="mb-4">
                  <Form.Label className="form-label">
                    <span className="label-icon">📞</span>
                    Phone Number
                  </Form.Label>
                  <Form.Control
                    type="tel"
                    name="phoneNumber"
                    value={formData.phoneNumber}
                    onChange={handleInputChange}
                    required
                    className="form-control-modern"
                    placeholder="Enter phone number"
                  />
                  <small className="text-muted">
                    Format: +1-234-567-8900 or 123-456-7890
                  </small>
                </Form.Group>
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
                    {isEditing ? 'Update Client' : 'Create Client'}
                  </>
                )}
              </Button>
              
              <Button 
                type="button" 
                variant="outline-secondary" 
                onClick={() => navigate('/clients')}
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

export default ClientForm; 