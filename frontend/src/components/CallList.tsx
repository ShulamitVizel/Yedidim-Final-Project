import React, { useState, useEffect } from 'react';
import { Table, Button, Badge, Modal, Alert, Card, Row, Col, Spinner } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { callApi } from '../services/api';
import { Call } from '../types';
import './CallList.css';

const CallList: React.FC = () => {
  const [calls, setCalls] = useState<Call[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [callToDelete, setCallToDelete] = useState<Call | null>(null);
  const [assigningCall, setAssigningCall] = useState<number | null>(null);

  useEffect(() => {
    fetchCalls();
  }, []);

  const fetchCalls = async () => {
    try {
      setLoading(true);
      const data = await callApi.getAllCalls();
      setCalls(data);
    } catch (err) {
      setError('Failed to fetch calls. Please check if the backend server is running.');
      console.error('Error fetching calls:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async () => {
    if (!callToDelete) return;

    try {
      await callApi.deleteCall(callToDelete.callId);
      setCalls(calls.filter(call => call.callId !== callToDelete.callId));
      setShowDeleteModal(false);
      setCallToDelete(null);
    } catch (err) {
      setError('Failed to delete call');
      console.error('Error deleting call:', err);
    }
  };

  const handleAssignBest = async (callId: number) => {
    try {
      setAssigningCall(callId);
      await callApi.assignBestVolunteer(callId);
      // Refresh the calls list to show updated assignment
      fetchCalls();
    } catch (err) {
      setError('Failed to assign best volunteer');
      console.error('Error assigning best volunteer:', err);
    } finally {
      setAssigningCall(null);
    }
  };

  if (loading) {
    return (
      <div className="call-list-loading">
        <div className="loading-spinner">
          <Spinner animation="border" variant="primary" />
          <p className="mt-3">Loading calls...</p>
        </div>
      </div>
    );
  }

  const activeCalls = calls.filter(call => call.finalVolunteerId === 0);
  const assignedCalls = calls.filter(call => call.finalVolunteerId !== 0);

  return (
    <div className="call-list-container">
      <div className="list-header">
        <div className="header-content">
          <h1 className="list-title">Emergency Calls</h1>
          <p className="list-subtitle">Manage and monitor all emergency calls</p>
        </div>
        <Link to="/calls/new" className="create-button">
          <span className="button-icon">➕</span>
          Create New Call
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
              <div className="stat-icon">📞</div>
              <div className="stat-content">
                <h3>{calls.length}</h3>
                <p>Total Calls</p>
              </div>
            </Card.Body>
          </Card>
        </Col>
        <Col lg={3} md={6} className="mb-3">
          <Card className="stat-card active">
            <Card.Body>
              <div className="stat-icon">🚨</div>
              <div className="stat-content">
                <h3>{activeCalls.length}</h3>
                <p>Active Calls</p>
              </div>
            </Card.Body>
          </Card>
        </Col>
        <Col lg={3} md={6} className="mb-3">
          <Card className="stat-card assigned">
            <Card.Body>
              <div className="stat-icon">✅</div>
              <div className="stat-content">
                <h3>{assignedCalls.length}</h3>
                <p>Assigned Calls</p>
              </div>
            </Card.Body>
          </Card>
        </Col>
        <Col lg={3} md={6} className="mb-3">
          <Card className="stat-card">
            <Card.Body>
              <div className="stat-icon">📊</div>
              <div className="stat-content">
                <h3>{calls.length > 0 ? Math.round((assignedCalls.length / calls.length) * 100) : 0}%</h3>
                <p>Assignment Rate</p>
              </div>
            </Card.Body>
          </Card>
        </Col>
      </Row>

      {calls.length === 0 ? (
        <Card className="empty-state-card">
          <Card.Body className="text-center">
            <div className="empty-icon">📞</div>
            <h3>No calls found</h3>
            <p className="text-muted">Get started by creating your first emergency call</p>
            <Link to="/calls/new" className="btn btn-primary btn-lg">
              Create Your First Call
            </Link>
          </Card.Body>
        </Card>
      ) : (
        <Card className="calls-table-card">
          <Card.Header>
            <h5 className="mb-0">All Calls</h5>
          </Card.Header>
          <Card.Body className="p-0">
            <div className="table-responsive">
              <Table className="calls-table">
                <thead>
                  <tr>
                    <th>ID</th>
                    <th>Type</th>
                    <th>Time</th>
                    <th>Client</th>
                    <th>Location</th>
                    <th>Status</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {calls.map((call) => (
                    <tr key={call.callId} className="call-row">
                      <td>
                        <span className="call-id">#{call.callId}</span>
                      </td>
                      <td>
                        <span className="call-type">{call.callType}</span>
                      </td>
                      <td>
                        <span className="call-time">
                          {new Date(call.callTime).toLocaleString()}
                        </span>
                      </td>
                      <td>
                        <span className="client-id">Client #{call.clientId}</span>
                      </td>
                      <td>
                        <span className="location">
                          {call.callLatitude.toFixed(4)}, {call.callLongitude.toFixed(4)}
                        </span>
                      </td>
                      <td>
                        {call.finalVolunteerId === 0 ? (
                          <Badge bg="warning" className="status-badge">
                            <span className="badge-icon">⏳</span>
                            Unassigned
                          </Badge>
                        ) : (
                          <Badge bg="success" className="status-badge">
                            <span className="badge-icon">✅</span>
                            Assigned to #{call.finalVolunteerId}
                          </Badge>
                        )}
                      </td>
                      <td>
                        <div className="action-buttons">
                          <Link 
                            to={`/calls/${call.callId}`} 
                            className="action-btn view-btn"
                            title="View Details"
                          >
                            👁️
                          </Link>
                          <Link 
                            to={`/calls/${call.callId}/edit`} 
                            className="action-btn edit-btn"
                            title="Edit Call"
                          >
                            ✏️
                          </Link>
                          {call.finalVolunteerId === 0 && (
                            <Button
                              variant="outline-success"
                              size="sm"
                              className="action-btn assign-btn"
                              onClick={() => handleAssignBest(call.callId)}
                              disabled={assigningCall === call.callId}
                              title="Auto-Assign Volunteer"
                            >
                              {assigningCall === call.callId ? (
                                <Spinner animation="border" size="sm" />
                              ) : (
                                '🤝'
                              )}
                            </Button>
                          )}
                          <Button
                            variant="outline-danger"
                            size="sm"
                            className="action-btn delete-btn"
                            onClick={() => {
                              setCallToDelete(call);
                              setShowDeleteModal(true);
                            }}
                            title="Delete Call"
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
            <p>Are you sure you want to delete Call #{callToDelete?.callId}?</p>
            <p className="text-muted">This action cannot be undone.</p>
          </div>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowDeleteModal(false)}>
            Cancel
          </Button>
          <Button variant="danger" onClick={handleDelete}>
            Delete Call
          </Button>
        </Modal.Footer>
      </Modal>
    </div>
  );
};

export default CallList; 