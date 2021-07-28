import React, { useState, useEffect } from 'react'
import { Col, Button, Form, FormGroup, Label, Input, Container, FormText, FormFeedback, Spinner, Alert, CustomInput, Row } from 'reactstrap';
import { aGet, aPost } from '../../utils/httpHelpers';

export default function ProductCreate() {
    const [isLoading, setIsLoading] = useState(false);
    const [message, setMessage] = useState('');
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [price, setPrice] = useState(0);
    const [stock, setStock] = useState(0);
    const [categories, setCategories] = useState([]);
    const [selectCateId, setSelectCateId] = useState(-1);
    const [subcategories, setSubcategories] = useState([]);
    const [selectSubcategoryId, setSelectSubcategoryId] = useState(-1);
    const [imgFiles, setImgFiles] = useState([]);
    const [imgUris, setImgUris] = useState([]);

    useEffect(() => {
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
                    setSelectCateId(1);
                    getSubcategoryByCategoryId(1);
                }
            })
            .catch((error) => {
                setMessage('Get Category list error!');
            })
            .finally(() => {
                setIsLoading(false);
            })
    }

    function getSubcategoryByCategoryId(id) {
        setIsLoading(true);

        aGet(`Category/${id}/subcategory`)
            .then(response => {
                setSubcategories(response.data);
                setSelectSubcategoryId(response.data[0].id);
            })
            .catch(error => {
                setMessage('Get subcategories list error');
            })
            .finally(() => {
                setIsLoading(false);
            })
    }

    function postProduct(event) {
        if (selectCateId != -1) {
            setIsLoading(true);

            let formData = new FormData();
            formData.set('Name', name);
            formData.set('Description', description);
            formData.set('Price', price);
            formData.set('Stock', stock);
            formData.set('SubcategoryId', selectSubcategoryId);
            formData.set('CategoryId', selectCateId);
            
            for (let i = 0; i < imgFiles.length; i++) {
                formData.append('Files', imgFiles[i]);
            }

            aPost('Product', formData)
            .then(response => {
                if (response.status === 204) {
                    setMessage('create product faild');
                } else {
                    setMessage(`Create product(${response.data.name}) successfully`);
                    setName('');
                    setDescription('');
                    setPrice(0);
                    setStock(0);
                }
            })
            .catch (error => {
                setMessage('An error occurred when create product');
            })
            .finally(() => {
                setIsLoading(false);
            })
        }
    }

    function handleInputEvent(event) {
        let value = event.target.value

        switch (event.target.name) {
            case 'Name':
                {
                    setName(value);
                    break;
                }
            case 'Description':
                {
                    setDescription(value);
                    break;
                }
            case 'Stock':
                {
                    setStock(value);
                    break;
                }
            case 'Price':
                {
                    setPrice(value);
                    break;
                }
            default:
                break;
        }

    }

    function onMessageDismiss() {
        setMessage('');
    }

    function handleSubmit(event) {
        event.preventDefault();

        postProduct(event);
    }

    function selectCategory(id) {
        setSelectCateId(id);

        getSubcategoryByCategoryId(id);
    }

    function onSelectCategory(event) {
        selectCategory(event.target.value);
    }

    function onSelectSubcategory(event) {
        setSelectSubcategoryId(event.target.value);
    }

    function onSelectImageFiles(event) {
        if (!event.target.files || event.target.files.length === 0) {
            loadShowImgs([]);
            return;
        }

        loadShowImgs(event.target.files);
    }

    function loadShowImgs(files) {
        setImgFiles(files);
        const tempImgUris = []

        for (let fi of files) {
            let reader = new FileReader();

            reader.onload = (event) => {
                tempImgUris.push(event.target.result);
            }

            reader.readAsDataURL(fi);
        }

        var waitLoadImage = setInterval(() => {
            if (tempImgUris.length >= files.length) {
                setImgUris(tempImgUris);
                clearInterval(waitLoadImage);
            }
        }, 1000) // Check load all image finish to reRender
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
                            id="productName"
                            placeholder="Enter Product Name"
                            value={name}
                            onChange={e => handleInputEvent(e)}
                            required />
                    </Col>
                </FormGroup>

                <FormGroup row className='mt-3'>
                    <Label for="productPrice" sm={2}>Price</Label>
                    <Col sm={10}>
                        <Input type="number"
                            name="Price"
                            id="productPrice"
                            placeholder="Enter Product Name"
                            value={price}
                            min="0"
                            oninput="validity.valid||(value='');"
                            onChange={e => handleInputEvent(e)}
                            required />
                    </Col>
                </FormGroup>

                <FormGroup row className='mt-3'>
                    <Label for="productStock" sm={2}>Stock</Label>
                    <Col sm={10}>
                        <Input type="number"
                            name="Stock"
                            id="productStock"
                            placeholder="Enter Product Name"
                            value={stock}
                            min="0"
                            oninput="validity.valid||(value='');"
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

                <FormGroup row className='mt-3' dark>
                    <Label for="Subcategory" sm={2}>Subcategory</Label>
                    <Col sm={10}>
                        <Input type="select" name="select" id="Subcategory" value={selectSubcategoryId} required onChange={e => onSelectSubcategory(e)}>
                            {
                                subcategories.length > 0 &&
                                subcategories.map(subCate => (
                                    <option value={subCate.id}>{subCate.name}</option>
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
                    <Label for="productUpload" sm={2}>Upload Images</Label>
                    <Col sm={10}>
                        <Input
                            type="file"
                            name="uploadImgFiles"
                            id="productUpload"
                            multiple="true"
                            onChange={e => onSelectImageFiles(e)}
                            accept="image/*"
                            required />
                    </Col>
                </FormGroup>

                <FormGroup row className='mt-3'>
                    <Col className='justify-content-center'>

                        <Button >Create</Button>
                    </Col>
                </FormGroup>
            </Form>

            {
                imgUris.length > 0 &&
                <Container className='my-5 border border-secondary' >
                    <h1 className='mt-3 justify-content-center'>Preview Images</h1>
                    <Row className='my-3'>
                        {
                            imgUris.map((urlData, index) => (
                                <Col key={index} xs='6' sm='3'>
                                    <img src={urlData} className="img-thumbnail" alt=''></img>
                                </Col>
                            ))
                        }
                    </Row>
                </Container>
            }
        </Container>
    )
}
