import React, { useEffect, useState, useStateCallback } from 'react'
import { useParams } from 'react-router-dom';
import { Col, Button, Form, FormGroup, Label, Input, Container, FormText, FormFeedback, Spinner, Alert } from 'reactstrap';
import { aGet, aPut } from '../../utils/httpHelpers';

export default function CategoryModifyItem() {
    const [isLoading, setIsLoading] = useState(false);
    const [message, setMessage] = useState('');
    const [cateId, setCateId] = useState(-1);
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');

    const { id } = useParams();

    useEffect(() => {
        setIsLoading(true);

        getCategory(id);
    }, [])

    function getCategory(_id) {
        aGet(`Category/${_id}`)
            .then(response => {
                if (response.status === 204) { //empty response
                    setMessage('Category not found')
                } else {
                    setCateId(response.data.id);
                    setName(response.data.name);
                    setDescription(response.data.description === null ? '' : response.data.description);
                }
            })
            .catch(error => {
                setMessage('Error occurred when load category data');
            })
            .finally(() => {
                setIsLoading(false);
            })
    }

    function putCategory() {
        let formData = new FormData();
        formData.set('name', name);
        formData.set('description', description);

        aPut(`Category/${cateId}`, formData)
            .then(response => {
                setCateId(response.data.id);
                setName(response.data.name);
                setDescription(response.data.description === null ? '' : response.data.description);
                setMessage('Update successfully');
            })
            .catch(error => {
                setMessage('Error occurred when load category data');
            })
            .finally(() => {
                setIsLoading(false);
            })
    }

    function handleInputEvent(event) {
        if (event.target.name === 'Name') {
            setName(event.target.value);
        }
        if (event.target.name === 'Description') {
            setDescription(event.target.value);
        }
    }

    function handleSubmit(event) {
        event.preventDefault();

        setIsLoading(true);

        putCategory();
    }

    function onMessageDismiss() {
        setMessage('');
    }

    return (
        <Container className='mt-3'>
            <h1 className='justify-content-center'>Create New Category:</h1>

            {isLoading &&
                <Container>
                    <Spinner type='grow' color='info' children='' />
                </Container>
            }

            {
                message.length > 0 &&
                <Alert color="success" isOpen={true} toggle={() => onMessageDismiss()}>
                    {message}
                </Alert>
            }

            <Form onSubmit={e => handleSubmit(e)}>
                <FormGroup row className='mt-3'>
                    <Label for="categoryName" sm={2}>Name</Label>
                    <Col sm={10}>
                        <Input type="text"
                            name="Name"
                            id="categoryName"
                            placeholder="Category's name should be unique"
                            value={name}
                            onChange={e => handleInputEvent(e)} />
                    </Col>
                </FormGroup>

                <FormGroup row className='mt-3'>
                    <Label for="categoryDescription" sm={2}>Description</Label>
                    <Col sm={10}>
                        <Input type="textarea"
                            name="Description"
                            id="categoryDescription"
                            value={description}
                            onChange={e => handleInputEvent(e)} />
                    </Col>
                </FormGroup>
                <FormGroup row className='mt-3'>
                    <Col className='justify-content-center'>

                        <Button >Update</Button>
                    </Col>
                </FormGroup>
            </Form>
        </Container>
    )
}
