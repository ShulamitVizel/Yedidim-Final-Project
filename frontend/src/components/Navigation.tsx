import React from 'react';
import { Navbar, Nav, Container, Badge } from 'react-bootstrap';
import { Link, useLocation } from 'react-router-dom';
import './Navigation.css';

const Navigation: React.FC = () => {
  const location = useLocation();

  const isActive = (path: string) => {
    return location.pathname === path;
  };

  return (
    <Navbar className="modern-navbar" expand="lg" fixed="top">
      <Container>
        <Navbar.Brand as={Link} to="/" className="navbar-brand">
          <span className="brand-icon">🚨</span>
          <span className="brand-text">Yedidim</span>
        </Navbar.Brand>
        
        <Navbar.Toggle aria-controls="basic-navbar-nav" className="navbar-toggler" />
        
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="ms-auto">
            <Nav.Link 
              as={Link} 
              to="/" 
              className={`nav-link ${isActive('/') ? 'active' : ''}`}
            >
              <span className="nav-icon">📊</span>
              Dashboard
            </Nav.Link>
            
            <Nav.Link 
              as={Link} 
              to="/calls" 
              className={`nav-link ${isActive('/calls') ? 'active' : ''}`}
            >
              <span className="nav-icon">📞</span>
              Calls
              <Badge bg="danger" className="nav-badge">New</Badge>
            </Nav.Link>
            
            <Nav.Link 
              as={Link} 
              to="/clients" 
              className={`nav-link ${isActive('/clients') ? 'active' : ''}`}
            >
              <span className="nav-icon">👥</span>
              Clients
            </Nav.Link>
            
            <Nav.Link 
              as={Link} 
              to="/volunteers" 
              className={`nav-link ${isActive('/volunteers') ? 'active' : ''}`}
            >
              <span className="nav-icon">🤝</span>
              Volunteers
            </Nav.Link>
            
            <Nav.Link 
              as={Link} 
              to="/calls/new" 
              className="nav-link create-call-btn"
            >
              <span className="nav-icon">➕</span>
              New Call
            </Nav.Link>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};

export default Navigation; 