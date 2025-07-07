import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { volunteerApi } from '../services/api';
import { Volunteer } from '../types';

const VolunteerDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [volunteer, setVolunteer] = useState<Volunteer | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!id) return;
    setLoading(true);
    volunteerApi.getVolunteerById(Number(id))
      .then(setVolunteer)
      .catch(() => setError('Volunteer not found or error loading data.'))
      .finally(() => setLoading(false));
  }, [id]);

  if (loading) return <div>Loading...</div>;
  if (error) return <div className="alert alert-danger">{error}</div>;
  if (!volunteer) return null;

  return (
    <div className="container mt-4">
      <h2>Volunteer Details</h2>
      <div className="card p-3 mb-3">
        <p><strong>Name:</strong> {volunteer.name}</p>
        <p><strong>Phone:</strong> {volunteer.phoneNumber}</p>
        <p><strong>Level:</strong> {volunteer.level}</p>
        <p><strong>Available:</strong> {volunteer.isAvailable ? 'Yes' : 'No'}</p>
        <p><strong>Location:</strong> ({volunteer.volunteerLatitude}, {volunteer.volunteerLongitude})</p>
      </div>
      <button className="btn btn-secondary" onClick={() => navigate(-1)}>Back</button>
    </div>
  );
};

export default VolunteerDetail; 