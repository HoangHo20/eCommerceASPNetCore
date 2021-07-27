import React, { Component } from 'react'
import {
    aGet,
    aPost,
} from '../../../utils/httpHelpers'
import { Col, Button, Form, FormGroup, Label, Input, Container, FormText, FormFeedback, Spinner, Alert } from 'reactstrap';
import axios from 'axios';

export default class index extends Component {
    state = {
        isExisted: false,
        Name: '',
        Description: '',
        isLoading: false,
        message: '',
        isMessageError: false
    }

    getCategory() {
        aGet("category").then((response) => {
            if (response.status === 200) {
                console.log(response.data)
            }
        });
    }

    postCategory(event, callback) {
        let formData = new FormData();
        formData.set('name', this.state.Name);
        formData.set('description', this.state.Description);

        aPost('category', formData)
        .then(response => {
            this.alertSuccess(`Add Category ${response.data.name} successfully`)
        })
        .catch(error => {
            console.log(error)
        })
        .finally(() => {
            callback();
        })

        
    }
    
    alertSuccess(message) {
        this.setState({
            message: message,
            isMessageError: false,
            Name: '',
            Description: ''
        })
    }

    alertError(message) {
        this.setState({
            message: 'Some Error occurred when checking Category Name',
            isMessageError: true
        })
    }

    handleChange(event) {
        const eValue = event.target.value;

        if (event.target.name === "Name") {
            this.setState({
                isExisted: false
            })
        }

        this.setState({
            [event.target.name]: eValue
        })

    }

    loadingSpinnerTrigger() {
        this.setState({
            isLoading: !this.state.isLoading
        })
    }

    handleSubmit(event) {
        event.preventDefault();
        console.log(this.state);

        this.setState({
            isLoading: !this.state.isLoading
        }, () => {
            aGet('category/name/' + this.state.Name)
                .then(response => {
                    if (response.status !== 204) {
                        this.setState({
                            isExisted: true
                        })

                        this.loadingSpinnerTrigger();
                    } else {
                        this.postCategory(event, () => this.loadingSpinnerTrigger())
                    }
                })
                .catch(error => {
                    console.log(error);
                    this.alertError("Some Error occurred when checking Category's Name")

                    this.loadingSpinnerTrigger();
                })
        })
    }

    onAlertDismiss() {
        this.setState({
            message: '',
            isMessageError: false
        })
    }

    render() {
        let nameInput;
        if (this.state.isExisted) {
            nameInput = <Input type="text"
                name="Name"
                id="categoryName"
                placeholder="Category's name should be unique"
                value={this.state.Name}
                onChange={e => this.handleChange(e)}
                invalid />
        } else {
            nameInput = <Input type="text"
                name="Name"
                id="categoryName"
                placeholder="Category's name should be unique"
                value={this.state.Name}
                onChange={e => this.handleChange(e)} />
        }

        let message = <></>;
        if (this.state.message.length > 0) {
            if (this.state.isMessageError) {
                message = <Alert color="danger" isOpen={true} toggle={() => this.onAlertDismiss()}>
                    {this.state.message}
                </Alert>
            } else {
                message = <Alert color="success" isOpen={true} toggle={() => this.onAlertDismiss()}>
                    {this.state.message}
                </Alert>
            }
        }

        return (
            <Container className='mt-3'>
                <h1 className='justify-content-center'>Create New Category:</h1>

                {message}

                {this.state.isLoading &&
                    <Container className='justify-content-center'>
                        <Spinner type='grow' color='info' children='' />
                    </Container>
                }

                <Form onSubmit={e => this.handleSubmit(e)}>
                    <FormGroup row className='mt-3'>
                        <Label for="categoryName" sm={2}>Name</Label>
                        <Col sm={10}>
                            {nameInput}
                            {this.state.isExisted &&
                                <FormFeedback>This name is already taken</FormFeedback>
                            }
                            <FormText>Category Name must be unique</FormText>
                        </Col>
                    </FormGroup>

                    <FormGroup row className='mt-3'>
                        <Label for="categoryDescription" sm={2}>Description</Label>
                        <Col sm={10}>
                            <Input type="textarea"
                                name="Description"
                                id="categoryDescription"
                                value={this.state.Description}
                                onChange={e => this.handleChange(e)} />
                        </Col>
                    </FormGroup>
                    <FormGroup row className='mt-3'>
                        <Col className='justify-content-center'>

                            <Button >Create</Button>
                        </Col>
                    </FormGroup>
                </Form>
            </Container>
        )
    }
}
