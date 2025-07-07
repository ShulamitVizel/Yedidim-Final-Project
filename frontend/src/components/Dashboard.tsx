import React, { useState, useEffect } from 'react';
import { Card, Row, Col, Badge, ProgressBar, Alert } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { callApi, volunteerApi } from '../services/api';
import { Call, Volunteer } from '../types';
import './Dashboard.css';

const Dashboard: React.FC = () => {
  const [calls, setCalls] = useState<Call[]>([]);
  const [volunteers, setVolunteers] = useState<Volunteer[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchDashboardData = async () => {
      try {
        setLoading(true);
        const [callsData, volunteersData] = await Promise.all([
          callApi.getAllCalls(),
          volunteerApi.getAllVolunteers()
        ]);
        
        setCalls(callsData);
        setVolunteers(volunteersData);
      } catch (error) {
        console.error('Error fetching dashboard data:', error);
        setError('Failed to load dashboard data. Please check if the backend server is running.');
      } finally {
        setLoading(false);
      }
    };

    fetchDashboardData();
  }, []);

  const activeCalls = calls.filter(call => call.finalVolunteerId === 0);
  const assignedCalls = calls.filter(call => call.finalVolunteerId !== 0);
  const availableVolunteers = volunteers.filter(volunteer => volunteer.isAvailable);
  const totalVolunteers = volunteers.length;
  const assignmentRate = totalVolunteers > 0 ? (assignedCalls.length / calls.length) * 100 : 0;

  if (loading) {
    return (
      <div className="dashboard-loading">
        <div className="loading-spinner">
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
          <p className="mt-3">Loading Yedidim Dashboard...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="dashboard-container">
      {error && (
        <Alert variant="danger" className="mb-4" onClose={() => setError(null)} dismissible>
          <Alert.Heading>Connection Error</Alert.Heading>
          <p>{error}</p>
          <hr />
          <p className="mb-0">
            Make sure your backend server is running on <code>https://localhost:7146</code>
          </p>
        </Alert>
      )}

      {/* Hero Section */}
      <div className="hero-section mb-5">
        <div className="hero-content">
          <h1 className="hero-title">Yedidim Emergency Response</h1>
          <p className="hero-subtitle">Connecting those in need with volunteers who care</p>
          <div className="hero-stats">
            <div className="stat-item">
              <span className="stat-number">{calls.length}</span>
              <span className="stat-label">Total Calls</span>
            </div>
            <div className="stat-item">
              <span className="stat-number">{availableVolunteers.length}</span>
              <span className="stat-label">Available Volunteers</span>
            </div>
            <div className="stat-item">
              <span className="stat-number">{Math.round(assignmentRate)}%</span>
              <span className="stat-label">Assignment Rate</span>
            </div>
          </div>
        </div>
      </div>

      {/* Statistics Cards */}
      <Row className="mb-5">
        <Col lg={3} md={6} className="mb-4">
          <Card className="stat-card total-calls">
            <Card.Body>
              <div className="stat-icon">📞</div>
              <Card.Title>Total Calls</Card.Title>
              <Card.Text className="stat-number">{calls.length}</Card.Text>
              <div className="stat-trend">
                <span className="trend-up">+12%</span> from last week
              </div>
            </Card.Body>
          </Card>
        </Col>
        <Col lg={3} md={6} className="mb-4">
          <Card className="stat-card active-calls">
            <Card.Body>
              <div className="stat-icon">🚨</div>
              <Card.Title>Active Calls</Card.Title>
              <Card.Text className="stat-number">{activeCalls.length}</Card.Text>
              <div className="stat-trend">
                <span className="trend-warning">Needs attention</span>
              </div>
            </Card.Body>
          </Card>
        </Col>
        <Col lg={3} md={6} className="mb-4">
          <Card className="stat-card assigned-calls">
            <Card.Body>
              <div className="stat-icon">✅</div>
              <Card.Title>Assigned Calls</Card.Title>
              <Card.Text className="stat-number">{assignedCalls.length}</Card.Text>
              <div className="stat-trend">
                <span className="trend-success">In progress</span>
              </div>
            </Card.Body>
          </Card>
        </Col>
        <Col lg={3} md={6} className="mb-4">
          <Card className="stat-card available-volunteers">
            <Card.Body>
              <div className="stat-icon">👥</div>
              <Card.Title>Available Volunteers</Card.Title>
              <Card.Text className="stat-number">{availableVolunteers.length}</Card.Text>
              <div className="stat-trend">
                <span className="trend-info">Ready to help</span>
              </div>
            </Card.Body>
          </Card>
        </Col>
      </Row>

      {/* Assignment Progress */}
      <Row className="mb-5">
        <Col>
          <Card className="progress-card">
            <Card.Header>
              <h5>Call Assignment Progress</h5>
            </Card.Header>
            <Card.Body>
              <div className="progress-info">
                <span>Assignment Rate</span>
                <span>{Math.round(assignmentRate)}%</span>
              </div>
              <ProgressBar 
                now={assignmentRate} 
                className="custom-progress"
                variant={assignmentRate > 80 ? "success" : assignmentRate > 50 ? "warning" : "danger"}
              />
              <div className="progress-details mt-3">
                <small className="text-muted">
                  {assignedCalls.length} of {calls.length} calls have been assigned to volunteers
                </small>
              </div>
            </Card.Body>
          </Card>
        </Col>
      </Row>

      {/* Quick Actions */}
      <Row className="mb-5">
        <Col>
          <Card className="action-card">
            <Card.Header>
              <h5>Quick Actions</h5>
            </Card.Header>
            <Card.Body>
              <Row>
                <Col lg={3} md={6} className="mb-3">
                  <Link to="/calls/new" className="action-button primary">
                    <div className="action-icon">➕</div>
                    <div className="action-content">
                      <h6>Create New Call</h6>
                      <small>Report an emergency</small>
                    </div>
                  </Link>
                </Col>
                <Col lg={3} md={6} className="mb-3">
                  <Link to="/clients/new" className="action-button success">
                    <div className="action-icon">👤</div>
                    <div className="action-content">
                      <h6>Add New Client</h6>
                      <small>Register a client</small>
                    </div>
                  </Link>
                </Col>
                <Col lg={3} md={6} className="mb-3">
                  <Link to="/volunteers/new" className="action-button info">
                    <div className="action-icon">🤝</div>
                    <div className="action-content">
                      <h6>Add New Volunteer</h6>
                      <small>Join our team</small>
                    </div>
                  </Link>
                </Col>
                <Col lg={3} md={6} className="mb-3">
                  <Link to="/calls" className="action-button secondary">
                    <div className="action-icon">📋</div>
                    <div className="action-content">
                      <h6>View All Calls</h6>
                      <small>Manage calls</small>
                    </div>
                  </Link>
                </Col>
              </Row>
            </Card.Body>
          </Card>
        </Col>
      </Row>

      {/* Recent Activity */}
      <Row>
        <Col lg={6} className="mb-4">
          <Card className="activity-card">
            <Card.Header>
              <h5>Recent Calls</h5>
              <Link to="/calls" className="view-all-link">View All</Link>
            </Card.Header>
            <Card.Body>
              {calls.slice(0, 5).map((call) => (
                <div key={call.callId} className="activity-item">
                  <div className="activity-icon">
                    {call.finalVolunteerId === 0 ? '🚨' : '✅'}
                  </div>
                  <div className="activity-content">
                    <div className="activity-title">
                      Call #{call.callId} - {call.callType}
                    </div>
                    <div className="activity-time">
                      {new Date(call.callTime).toLocaleString()}
                    </div>
                  </div>
                  <div className="activity-status">
                    {call.finalVolunteerId === 0 ? (
                      <Badge bg="warning">Unassigned</Badge>
                    ) : (
                      <Badge bg="success">Assigned</Badge>
                    )}
                  </div>
                </div>
              ))}
              {calls.length === 0 && (
                <div className="empty-state">
                  <div className="empty-icon">📞</div>
                  <p>No calls found</p>
                  <Link to="/calls/new" className="btn btn-primary btn-sm">
                    Create First Call
                  </Link>
                </div>
              )}
            </Card.Body>
          </Card>
        </Col>
        
        <Col lg={6} className="mb-4">
          <Card className="activity-card">
            <Card.Header>
              <h5>Available Volunteers</h5>
              <Link to="/volunteers" className="view-all-link">View All</Link>
            </Card.Header>
            <Card.Body>
              {availableVolunteers.slice(0, 5).map((volunteer) => (
                <div key={volunteer.volunteerId} className="activity-item">
                  <div className="activity-icon">👤</div>
                  <div className="activity-content">
                    <div className="activity-title">{volunteer.name}</div>
                    <div className="activity-time">Level: {volunteer.level.trim()}</div>
                  </div>
                  <div className="activity-status">
                    <Badge bg="info">{volunteer.phoneNumber}</Badge>
                  </div>
                </div>
              ))}
              {availableVolunteers.length === 0 && (
                <div className="empty-state">
                  <div className="empty-icon">👥</div>
                  <p>No available volunteers</p>
                  <Link to="/volunteers/new" className="btn btn-primary btn-sm">
                    Add Volunteer
                  </Link>
                </div>
              )}
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </div>
  );
};

export default Dashboard; 