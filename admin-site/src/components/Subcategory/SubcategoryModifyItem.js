import React, { useState, useEffect } from 'react'
import { Col, Button, Form, FormGroup, Label, Input, Container, FormText, FormFeedback, Spinner, Alert } from 'reactstrap';
import { aGet, aPut } from '../../utils/httpHelpers';
import {useParams} from 'react-router-dom'

export default function SubcategoryModifyItem() {
    const [isLoading, setIsLoading] = useState(false);
    const [message, setMessage] = useState('');
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [categories, setCategories] = useState([]);
    const [selectCateId, setSelectCateId] = useState(-1);

    const {id} = useParams();

    useEffect(() => {
        getSubcategoryById(id);

        getAllCategories();
    }, [])

    function getAllCategories() {
        setIsLoading(true);

        aGet('Category')
            .then(response => {
                if (response === 204) {//empty response
                    setMessage("Please create a Category first")
                } else {
                    setCategories(response.data);
                }
            })
            .catch((error) => {
                setMessage('Get Category lis error!');
            })
            .finally(() => {
                setIsLoading(false);
            })
    }

    function getSubcategoryById(subId) {
        setIsLoading(true);

        aGet(`Category/subcategory/${subId}`)
        .then(response => {
            if (response.status === 204) {
                setMessage('Subcategory not found');
            } else {
                setName(response.data.name);
                setDescription(response.data.description);
                setSelectCateId(response.data.categoryId);
            }
        })
        .catch(error => {
            setMessage('Get subcategory data error');
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

    function putSubcategory() {
        if (selectCateId !== -1) {
            setIsLoading(true);

            let formData = new FormData();
            formData.set('name', name);
            formData.set('description', description);

            aPut(`Category/${selectCateId}/subcategory/${id}`, formData)
                .then(response => {
                    if (response.status !== 204) { // has response body
                        let selectedCategory = categories.find(c => c.id === response.data.categoryId);

                        setMessage(`update Subcategory(${response.data.name}) in Category(${selectedCategory.name}) successfully`);
                    }
                })
                .catch(error => {
                    setMessage('update subcategory failed');
                })
                .finally(() => {
                    setIsLoading(false);
                })
        }
    }

    function onMessageDismiss() {
        setMessage('');
    }

    function handleSubmit(event) {
        event.preventDefault();

        putSubcategory();
    }

    function onSelectCategory(event) {
        setSelectCateId(event.target.value);
    }

    return (
        <Container className='mt-3'>
            <h1 className='justify-content-center'>Create New Subcategory:</h1>

            {isLoading &&
                <Container className="justify-content-center" >
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
                            onChange={e => handleInputEvent(e)}
                            required />
                    </Col>
                </FormGroup>

                <FormGroup row className='mt-3' dark>
                    <Label for="Category" sm={2}>Category</Label>
                    <Col sm={10}>
                        <Input type="select" name="select" id="Category" value={selectCateId} required onChange={e => onSelectCategory(e)}>
                            {
                                categories.length > 0 &&
                                categories.map(cate => (
                                    <option value={cate.id}>{cate.name}</option>
                                ))
                            }
                        </Input>
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
