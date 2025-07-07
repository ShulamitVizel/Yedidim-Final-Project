import React, { useState, useEffect } from 'react';
import { Table, Button, Badge, Modal, Alert, Card, Row, Col, Spinner } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { volunteerApi } from '../services/api';
import { Volunteer } from '../types';
import './VolunteerList.css';

const VolunteerList: React.FC = () => {
  const [volunteers, setVolunteers] = useState<Volunteer[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [volunteerToDelete, setVolunteerToDelete] = useState<Volunteer | null>(null);

  useEffect(() => {
    fetchVolunteers();
  }, []);

  const fetchVolunteers = async () => {
    try {
      setLoading(true);
      const data = await volunteerApi.getAllVolunteers();
      setVolunteers(data);
    } catch (err) {
      setError('Failed to fetch volunteers. Please check if the backend server is running.');
      console.error('Error fetching volunteers:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async () => {
    if (!volunteerToDelete) return;

    try {
      await volunteerApi.deleteVolunteer(volunteerToDelete.volunteerId);
      setVolunteers(volunteers.filter(volunteer => volunteer.volunteerId !== volunteerToDelete.volunteerId));
      setShowDeleteModal(false);
      setVolunteerToDelete(null);
    } catch (err) {
      setError('Failed to delete volunteer');
      console.error('Error deleting volunteer:', err);
    }
  };

  if (loading) {
    return (
      <div className="volunteer-list-loading">
        <div className="loading-spinner">
          <Spinner animation="border" variant="primary" />
          <p className="mt-3">Loading volunteers...</p>
        </div>
      </div>
    );
  }

  const availableVolunteers = volunteers.filter(v => v.isAvailable);
  const busyVolunteers = volunteers.filter(v => !v.isAvailable);

  return (
    <div className="volunteer-list-container">
      <div className="list-header">
        <div className="header-content">
          <h1 className="list-title">Volunteers</h1>
          <p className="list-subtitle">Manage and monitor all registered volunteers</p>
        </div>
        <Link to="/volunteers/new" className="create-button">
          <span className="button-icon">➕</span>
          Add New Volunteer
        </Link>
      </div>

      {error && (
        <Alert variant="danger" className="mb-4" onClose={() => setError(null)} dismissible>
          <Alert.Heading>Error</Alert.Heading>
          {error}
        </Alert>
      )}

      {/* Statistics Cards */}
      <Row className="mb-4">
        <Col lg={3} md={6} className="mb-3">
          <Card className="stat-card">
            <Card.Body>
              <div className="stat-icon">🤝</div>
              <div className="stat-content">
                <h3>{volunteers.length}</h3>
                <p>Total Volunteers</p>
              </div>
            </Card.Body>
          </Card>
        </Col>
        <Col lg={3} md={6} className="mb-3">
          <Card className="stat-card active">
            <Card.Body>
              <div className="stat-icon">✅</div>
              <div className="stat-content">
                <h3>{availableVolunteers.length}</h3>
                <p>Available</p>
              </div>
            </Card.Body>
          </Card>
        </Col>
        <Col lg={3} md={6} className="mb-3">
          <Card className="stat-card assigned">
            <Card.Body>
              <div className="stat-icon">🚫</div>
              <div className="stat-content">
                <h3>{busyVolunteers.length}</h3>
                <p>Busy</p>
              </div>
            </Card.Body>
          </Card>
        </Col>
        <Col lg={3} md={6} className="mb-3">
          <Card className="stat-card">
            <Card.Body>
              <div className="stat-icon">📊</div>
              <div className="stat-content">
                <h3>{volunteers.filter(v => v.calls && v.calls.length > 0).length}</h3>
                <p>With Calls</p>
              </div>
            </Card.Body>
          </Card>
        </Col>
      </Row>

      {volunteers.length === 0 ? (
        <Card className="empty-state-card">
          <Card.Body className="text-center">
            <div className="empty-icon">🤝</div>
            <h3>No volunteers found</h3>
            <p className="text-muted">Get started by adding your first volunteer</p>
            <Link to="/volunteers/new" className="btn btn-primary btn-lg">
              Add Your First Volunteer
            </Link>
          </Card.Body>
        </Card>
      ) : (
        <Card className="volunteers-table-card">
          <Card.Header>
            <h5 className="mb-0">All Volunteers</h5>
          </Card.Header>
          <Card.Body className="p-0">
            <div className="table-responsive">
              <Table className="volunteers-table">
                <thead>
                  <tr>
                    <th>ID</th>
                    <th>Name</th>
                    <th>Level</th>
                    <th>Phone</th>
                    <th>Location</th>
                    <th>Status</th>
                    <th>Calls</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {volunteers.map((volunteer) => (
                    <tr key={volunteer.volunteerId} className="volunteer-row">
                      <td>
                        <span className="volunteer-id">#{volunteer.volunteerId}</span>
                      </td>
                      <td>
                        <span className="volunteer-name">{volunteer.name}</span>
                      </td>
                      <td>
                        <Badge bg="info" className="level-badge">
                          {volunteer.level.trim()}
                        </Badge>
                      </td>
                      <td>
                        <span className="volunteer-phone">{volunteer.phoneNumber}</span>
                      </td>
                      <td>
                        <span className="location">
                          {volunteer.volunteerLatitude.toFixed(4)}, {volunteer.volunteerLongitude.toFixed(4)}
                        </span>
                      </td>
                      <td>
                        {volunteer.isAvailable ? (
                          <Badge bg="success" className="status-badge">
                            <span className="badge-icon">✅</span>
                            Available
                          </Badge>
                        ) : (
                          <Badge bg="warning" className="status-badge">
                            <span className="badge-icon">🚫</span>
                            Busy
                          </Badge>
                        )}
                      </td>
                      <td>
                        <span className="volunteer-calls">
                          <Badge bg="info">{volunteer.calls?.length || 0} calls</Badge>
                        </span>
                      </td>
                      <td>
                        <div className="action-buttons">
                          <Link 
                            to={`/volunteers/${volunteer.volunteerId}`} 
                            className="action-btn view-btn"
                            title="View Details"
                          >
                            👁️
                          </Link>
                          <Link 
                            to={`/volunteers/${volunteer.volunteerId}/edit`} 
                            className="action-btn edit-btn"
                            title="Edit Volunteer"
                          >
                            ✏️
                          </Link>
                          <Button
                            variant="outline-danger"
                            size="sm"
                            className="action-btn delete-btn"
                            onClick={() => {
                              setVolunteerToDelete(volunteer);
                              setShowDeleteModal(true);
                            }}
                            title="Delete Volunteer"
                          >
                            🗑️
                          </Button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </Table>
            </div>
          </Card.Body>
        </Card>
      )}

      {/* Delete Confirmation Modal */}
      <Modal show={showDeleteModal} onHide={() => setShowDeleteModal(false)} className="delete-modal">
        <Modal.Header closeButton>
          <Modal.Title>Confirm Delete</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <div className="delete-warning">
            <div className="warning-icon">⚠️</div>
            <p>Are you sure you want to delete Volunteer #{volunteerToDelete?.volunteerId} ({volunteerToDelete?.name})?</p>
            <p className="text-muted">This action cannot be undone.</p>
          </div>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowDeleteModal(false)}>
            Cancel
          </Button>
          <Button variant="danger" onClick={handleDelete}>
            Delete Volunteer
          </Button>
        </Modal.Footer>
      </Modal>
    </div>
  );
};

export default VolunteerList; 