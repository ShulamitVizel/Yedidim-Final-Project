import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navigation from './components/Navigation';
import Dashboard from './components/Dashboard';
import CallList from './components/CallList';
import CallForm from './components/CallForm';
import ClientList from './components/ClientList';
import ClientForm from './components/ClientForm';
import VolunteerList from './components/VolunteerList';
import VolunteerForm from './components/VolunteerForm';
import VolunteerDetail from './components/VolunteerDetail';
import NotFound from './components/NotFound';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';

// Placeholder components
const CallDetail = () => <div className="placeholder">Call Detail - Coming Soon</div>;

function App() {
  return (
    <Router>
      <div className="App">
        <Navigation />
        <main className="main-content">
          <Routes>
            <Route path="/" element={<Dashboard />} />
            <Route path="/calls" element={<CallList />} />
            <Route path="/calls/new" element={<CallForm />} />
            <Route path="/calls/:id" element={<CallDetail />} />
            <Route path="/calls/:id/edit" element={<CallForm />} />
            <Route path="/clients" element={<ClientList />} />
            <Route path="/clients/new" element={<ClientForm />} />
            <Route path="/clients/:id/edit" element={<ClientForm />} />
            <Route path="/volunteers" element={<VolunteerList />} />
            <Route path="/volunteers/new" element={<VolunteerForm />} />
            <Route path="/volunteers/:id" element={<VolunteerDetail />} />
            <Route path="/volunteers/:id/edit" element={<VolunteerForm />} />
            <Route path="*" element={<NotFound />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
