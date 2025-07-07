import React, { useState, useEffect } from 'react';
import { Table, Button, Badge, Modal, Alert, Card, Row, Col, Spinner } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { clientApi } from '../services/api';
import { Client } from '../types';
import './ClientList.css';

const ClientList: React.FC = () => {
  const [clients, setClients] = useState<Client[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [clientToDelete, setClientToDelete] = useState<Client | null>(null);

  useEffect(() => {
    fetchClients();
  }, []);

  const fetchClients = async () => {
    try {
      setLoading(true);
      const data = await clientApi.getAllClients();
      setClients(data);
    } catch (err) {
      setError('Failed to fetch clients. Please check if the backend server is running.');
      console.error('Error fetching clients:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async () => {
    if (!clientToDelete) return;

    try {
      await clientApi.deleteClient(clientToDelete.clientId);
      setClients(clients.filter(client => client.clientId !== clientToDelete.clientId));
      setShowDeleteModal(false);
      setClientToDelete(null);
    } catch (err) {
      setError('Failed to delete client');
      console.error('Error deleting client:', err);
    }
  };

  if (loading) {
    return (
      <div className="client-list-loading">
        <div className="loading-spinner">
          <Spinner animation="border" variant="primary" />
          <p className="mt-3">Loading clients...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="client-list-container">
      <div className="list-header">
        <div className="header-content">
          <h1 className="list-title">Clients</h1>
          <p className="list-subtitle">Manage and monitor all registered clients</p>
        </div>
        <Link to="/clients/new" className="create-button">
          <span className="button-icon">➕</span>
          Add New Client
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
        <Col lg={4} md={6} className="mb-3">
          <Card className="stat-card">
            <Card.Body>
              <div className="stat-icon">👥</div>
              <div className="stat-content">
                <h3>{clients.length}</h3>
                <p>Total Clients</p>
              </div>
            </Card.Body>
          </Card>
        </Col>
        <Col lg={4} md={6} className="mb-3">
          <Card className="stat-card active">
            <Card.Body>
              <div className="stat-icon">📞</div>
              <div className="stat-content">
                <h3>{clients.filter(c => c.phoneNumber).length}</h3>
                <p>With Phone</p>
              </div>
            </Card.Body>
          </Card>
        </Col>
        <Col lg={4} md={6} className="mb-3">
          <Card className="stat-card assigned">
            <Card.Body>
              <div className="stat-icon">📊</div>
              <div className="stat-content">
                <h3>{clients.filter(c => c.calls && c.calls.length > 0).length}</h3>
                <p>With Calls</p>
              </div>
            </Card.Body>
          </Card>
        </Col>
      </Row>

      {clients.length === 0 ? (
        <Card className="empty-state-card">
          <Card.Body className="text-center">
            <div className="empty-icon">👥</div>
            <h3>No clients found</h3>
            <p className="text-muted">Get started by adding your first client</p>
            <Link to="/clients/new" className="btn btn-primary btn-lg">
              Add Your First Client
            </Link>
          </Card.Body>
        </Card>
      ) : (
        <Card className="clients-table-card">
          <Card.Header>
            <h5 className="mb-0">All Clients</h5>
          </Card.Header>
          <Card.Body className="p-0">
            <div className="table-responsive">
              <Table className="clients-table">
                <thead>
                  <tr>
                    <th>ID</th>
                    <th>Name</th>
                    <th>Phone</th>
                    <th>Calls</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {clients.map((client) => (
                    <tr key={client.clientId} className="client-row">
                      <td>
                        <span className="client-id">#{client.clientId}</span>
                      </td>
                      <td>
                        <span className="client-name">{client.name}</span>
                      </td>
                      <td>
                        <span className="client-phone">
                          {client.phoneNumber || <Badge bg="secondary">Not provided</Badge>}
                        </span>
                      </td>
                      <td>
                        <span className="client-calls">
                          <Badge bg="info">{client.calls?.length || 0} calls</Badge>
                        </span>
                      </td>
                      <td>
                        <div className="action-buttons">
                          <Link 
                            to={`/clients/${client.clientId}`} 
                            className="action-btn view-btn"
                            title="View Details"
                          >
                            👁️
                          </Link>
                          <Link 
                            to={`/clients/${client.clientId}/edit`} 
                            className="action-btn edit-btn"
                            title="Edit Client"
                          >
                            ✏️
                          </Link>
                          <Button
                            variant="outline-danger"
                            size="sm"
                            className="action-btn delete-btn"
                            onClick={() => {
                              setClientToDelete(client);
                              setShowDeleteModal(true);
                            }}
                            title="Delete Client"
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
            <p>Are you sure you want to delete Client #{clientToDelete?.clientId} ({clientToDelete?.name})?</p>
            <p className="text-muted">This action cannot be undone.</p>
          </div>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowDeleteModal(false)}>
            Cancel
          </Button>
          <Button variant="danger" onClick={handleDelete}>
            Delete Client
          </Button>
        </Modal.Footer>
      </Modal>
    </div>
  );
};

export default ClientList; 