import React, { useState, useEffect } from 'react';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Modal from 'react-bootstrap/Modal';
import Table from 'react-bootstrap/Table';
import Alert from 'react-bootstrap/Alert';
import Spinner from 'react-bootstrap/Spinner';
import axios from 'axios';

import AddSubscriptionForm from './AddSubscriptionForm';

const BASE_URL = 'http://192.168.49.2:30005';

const SubscriptionTable = () => {
  const [subscriptions, setSubscriptions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showAddModal, setShowAddModal] = useState(false);

  useEffect(() => {
    loadSubscriptions();
  }, []);

  const loadSubscriptions = () => {
    setLoading(true);
    axios.get(`${BASE_URL}/api/subs`)
        .then(response => {
          setSubscriptions(response.data);
          setLoading(false);
        })
        .catch(error => {
          setError(error.message);
          setLoading(false);
        });
  };

  const addSubscription = (newSub) => {
    setSubscriptions([...subscriptions, newSub]);
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Confirm deletion of this subscription?')) return;

    try {
      await axios.delete(`${BASE_URL}/api/subs/${id}`);
      setSubscriptions(prevSubs => prevSubs.filter(sub => sub.id !== id));
    } catch (err) {
      alert('Error deleting subscription.');
    }
  };

  return (
      <>
        {loading && <Spinner animation="border" role="status"><span className="visually-hidden">Loading...</span></Spinner>}
        {!loading && error && <Alert variant="danger">{error}</Alert>}
        {!loading && !error &&
            <>
              <Button variant="success" onClick={() => setShowAddModal(true)}>
                Add Subscription
              </Button>

              <br /><br />

              <Table striped bordered hover size="sm">
                <thead>
                <tr>
                  <th>ID</th>
                  <th>User</th>
                  <th>Subscription Type</th>
                  <th>Start Date</th>
                  <th>End Date</th>
                  <th>Active?</th>
                  <th>Actions</th>
                </tr>
                </thead>
                <tbody>
                {subscriptions.map(sub => (
                    <tr key={sub.id}>
                      <td>{sub.id}</td>
                      <td>{sub.userId}</td>
                      <td>{sub.subscriptionTypeId}</td>
                      <td>{sub.startDate}</td>
                      <td>{sub.endDate}</td>
                      <td>{sub.isActive ? 'Yes' : 'No'}</td>
                      <td>
                        <Button variant="danger" onClick={() => handleDelete(sub.id)}>Delete</Button>
                      </td>
                    </tr>
                ))}
                </tbody>
              </Table>

              <Modal show={showAddModal} onHide={() => setShowAddModal(false)}>
                <Modal.Header closeButton>
                  <Modal.Title>Add New Subscription</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                  <AddSubscriptionForm onSubmit={addSubscription} />
                </Modal.Body>
              </Modal>
            </>
        }
      </>
  );
};

export default SubscriptionTable;