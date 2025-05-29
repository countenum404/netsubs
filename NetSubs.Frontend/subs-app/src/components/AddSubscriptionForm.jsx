import React, { useState } from 'react';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';

const BASE_URL = 'http://192.168.49.2:30005';

const AddSubscriptionForm = ({ onSubmit }) => {
    const [req, setReq] = useState({
        userGuid: '',
        subscriptionGuid: '', 
        start: '',
        end: ''
    });

    const handleSubmit = async (event) => {
        event.preventDefault();

        try {
            const response = await fetch(`${BASE_URL}/api/subs`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(req),
            });

            if (response.ok) {
                const data = await response.json();
                onSubmit(data);
                setReq({}); // Сбрасываем форму
            } else {
                throw new Error('Server error');
            }
        } catch (err) {
            alert(err.message || 'Something went wrong');
        }
    };

    return (
        <Form onSubmit={handleSubmit}>
            <Form.Group controlId="formUserGUID">
                <Form.Label>User GUID:</Form.Label>
                <Form.Control
                    type="text"
                    value={req.userGuid}
                    onChange={(e) => setReq({ ...req, userGuid: e.target.value })}
                />
            </Form.Group>
            <Form.Group controlId="formSubscriptionType">
                <Form.Label>Subscription Type GUID:</Form.Label>
                <Form.Control
                    type="text"
                    value={req.subscriptionGuid}
                    onChange={(e) => setReq({ ...req, subscriptionGuid: e.target.value })}
                />
            </Form.Group>
            <Form.Group controlId="formStartDate">
                <Form.Label>Start Date:</Form.Label>
                <Form.Control
                    type="date"
                    value={req.start}
                    onChange={(e) => setReq({ ...req, start: new Date(e.target.value) })}
                />
            </Form.Group>
            <Form.Group controlId="formEndDate">
                <Form.Label>End Date:</Form.Label>
                <Form.Control
                    type="date"
                    value={req.end}
                    onChange={(e) => setReq({ ...req, end: new Date(e.target.value) })}
                />
            </Form.Group>
            <Button variant="primary" type="submit">Create Subscription</Button>
        </Form>
    );
};

export default AddSubscriptionForm;