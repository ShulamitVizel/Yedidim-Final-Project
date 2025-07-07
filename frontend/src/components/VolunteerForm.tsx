import React, { useState, useEffect } from 'react';
import { Form, Button, Card, Alert, Row, Col, Spinner } from 'react-bootstrap';
import { useParams, useNavigate } from 'react-router-dom';
import { volunteerApi } from '../services/api';
import { Volunteer } from '../types';
import './VolunteerForm.css';

const VolunteerForm: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const isEditing = !!id;

  const [formData, setFormData] = useState<Omit<Volunteer, 'volunteerId' | 'calls' | 'callsNavigation'>>({
    name: '',
    level: '',
    isAvailable: true,
    phoneNumber: '',
    volunteerLatitude: 32.0853,
    volunteerLongitude: 34.7818,
  });

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  useEffect(() => {
    if (isEditing && id) {
      fetchVolunteer(parseInt(id));
    }
  }, [id, isEditing]);

  const fetchVolunteer = async (volunteerId: number) => {
    try {
      setLoading(true);
      const volunteer = await volunteerApi.getVolunteerById(volunteerId);
      setFormData({
        name: volunteer.name,
        level: volunteer.level,
        isAvailable: volunteer.isAvailable,
        phoneNumber: volunteer.phoneNumber,
        volunteerLatitude: volunteer.volunteerLatitude,
        volunteerLongitude: volunteer.volunteerLongitude,
      });
    } catch (err) {
      setError('Failed to fetch volunteer');
      console.error('Error fetching volunteer:', err);
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
        await volunteerApi.updateVolunteer(parseInt(id), formData);
        setSuccess('Volunteer updated successfully!');
      } else {
        await volunteerApi.createVolunteer(formData);
        setSuccess('Volunteer created successfully!');
      }
      
      // Redirect after a short delay to show success message
      setTimeout(() => {
        navigate('/volunteers');
      }, 1500);
    } catch (err) {
      setError('Failed to save volunteer. Please check your connection and try again.');
      console.error('Error saving volunteer:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<any>) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : 
              type === 'number' ? parseFloat(value) || 0 : value,
    }));
  };

  const handleLocationClick = () => {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          setFormData(prev => ({
            ...prev,
            volunteerLatitude: position.coords.latitude,
            volunteerLongitude: position.coords.longitude,
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
          <p className="mt-3">Loading volunteer details...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="volunteer-form-container">
      <div className="form-header">
        <h1 className="form-title">
          {isEditing ? 'Edit Volunteer' : 'Add New Volunteer'}
        </h1>
        <p className="form-subtitle">
          {isEditing ? 'Update volunteer information' : 'Register a new volunteer in the system'}
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
                    Volunteer Name
                  </Form.Label>
                  <Form.Control
                    type="text"
                    name="name"
                    value={formData.name}
                    onChange={handleInputChange}
                    required
                    className="form-control-modern"
                    placeholder="Enter volunteer's full name"
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
                </Form.Group>
              </Col>
            </Row>

            <Row>
              <Col lg={6}>
                <Form.Group className="mb-4">
                  <Form.Label className="form-label">
                    <span className="label-icon">⭐</span>
                    Level
                  </Form.Label>
                  <Form.Select
                    name="level"
                    value={formData.level}
                    onChange={handleInputChange}
                    required
                    className="form-control-modern"
                  >
                    <option value="">Select level...</option>
                    <option value="Beginner">Beginner</option>
                    <option value="Intermediate">Intermediate</option>
                    <option value="Advanced">Advanced</option>
                    <option value="Expert">Expert</option>
                  </Form.Select>
                </Form.Group>
              </Col>
              
              <Col lg={6}>
                <Form.Group className="mb-4">
                  <Form.Label className="form-label">
                    <span className="label-icon">🔄</span>
                    Availability Status
                  </Form.Label>
                  <div className="availability-toggle">
                    <Form.Check
                      type="switch"
                      id="availability-switch"
                      name="isAvailable"
                      checked={formData.isAvailable}
                      onChange={handleInputChange}
                      label={formData.isAvailable ? "Available" : "Busy"}
                      className="modern-switch"
                    />
                  </div>
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
                          name="volunteerLatitude"
                          value={formData.volunteerLatitude}
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
                          name="volunteerLongitude"
                          value={formData.volunteerLongitude}
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
                    {isEditing ? 'Update Volunteer' : 'Create Volunteer'}
                  </>
                )}
              </Button>
              
              <Button 
                type="button" 
                variant="outline-secondary" 
                onClick={() => navigate('/volunteers')}
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

export default VolunteerForm; 