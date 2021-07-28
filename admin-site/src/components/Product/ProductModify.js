import React, { useEffect, useState } from 'react'
import { Link, Redirect, useParams } from 'react-router-dom'
import { Container, Table, Alert, Button, Spinner, FormGroup, Label, Col, Input } from 'reactstrap'
import { aDelete, aGet } from '../../utils/httpHelpers';

export default function ProductModify() {
    const [isLoading, setIsLoading] = useState(false);
    const [message, setMessage] = useState('');
    const [categories, setCategories] = useState([]);
    const [selectCateId, setSelectCateId] = useState(-1);
    const [subcategories, setSubcategories] = useState([]);
    const [selectSubcategoryId, setSelectSubcategoryId] = useState(-1);
    const [products, setProducts] = useState([]);

    useEffect(() => {
        getAllCategory();
    }, [])

    function getAllCategory() {
        setIsLoading(true);

        aGet('Category')
            .then(response => {
                if (response.status === 204) { //response body empty
                    setMessage('No category found');
                } else {
                    setCategories(response.data);
                    setSelectCateId(response.data[0].id);
                    getSubcategoryByCategoryId(response.data[0].id);
                }
            })
            .catch(error => {
                setMessage('Cannot get categories list');
            })
            .finally(() => {
                setIsLoading(false);
            })
    }

    function getSubcategoryByCategoryId(id) {
        setIsLoading(true);

        aGet(`Category/${id}/subcategory`)
            .then(response => {
                if (response.status === 204 || response.data.length < 1) {
                    setMessage('Subcategory list empty')
                    setSubcategories(response.data);
                    setProducts([]);
                } else {
                    setSubcategories(response.data);
                    getProductsBySubcategoryId(response.data[0].id)
                }
            })
            .catch(error => {
                setMessage('Get subcategories list error');
            })
            .finally(() => {
                setIsLoading(false);
            })
    }

    function getProductsBySubcategoryId(id) {
        setIsLoading(true);

        aGet(`Category/subcategory/${id}/product`)
            .then(response => {
                setProducts(response.data);
            })
            .catch(error => {
                setMessage('Load product list error');
            })
            .finally(() => {
                setIsLoading(false);
            })
    }

    function deleteProduct(id) {
        setIsLoading(true);

        aDelete(`Product/${id}`)
            .then(response => {
                if (response.data) {
                    let deletedID = response.data.id;
                    let deletedProduct = products.find(p => p.id === deletedID);

                    setMessage(`Delete (${deletedProduct.name}) completed`);
                    setProducts(products.filter(prod => prod !== deletedProduct));
                }
            })
            .catch(error => {
                setMessage('Cannot delete the product');
            })
            .finally(() => {
                setIsLoading(false);
            })
    }

    function onMessageDismiss() {
        setMessage('');
    }

    function selectCategory(id) {
        setSelectCateId(id);

        getSubcategoryByCategoryId(id);
    }

    function onSelectCategory(event) {
        selectCategory(event.target.value);
    }

    function onSelectSubcategory(event) {
        setSelectSubcategoryId(event.target.value)

        getProductsBySubcategoryId(event.target.value);
    }

    function onDeleteClick(productID) {
        let selectProduct = products.find(prod => prod.id === productID);

        if (window.confirm(`Are you sure delete (${selectProduct.name})?`)) {
            // confirm delete
            deleteProduct(productID);
        }
    }

    function productTable() {
        return (
            <Table hover className="mt-3 react-table">
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Price</th>
                        <th>Stock</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {
                        products.map(prd => (
                            <tr key={prd.id} className="react-table-row">
                                <th scope="row">{prd.id}</th>
                                <td>{prd.name}</td>
                                <td>{prd.description}</td>
                                <td>{prd.price}</td>
                                <td>{prd.stock}</td>
                                <td>
                                    <Link to={`/product/modify/${prd.id}`}>
                                        <Button color='info' className="react-table-row-action" >Edit</Button>
                                    </Link>
                                    <Button
                                        color='danger'
                                        className="react-table-row-action"
                                        onClick={() => onDeleteClick(prd.id)} >
                                        Delete
                                    </Button>
                                </td>
                            </tr>
                        ))
                    }
                </tbody>
            </Table>
        )
    }


    return (
        <Container className='mt-5'>
            <h1 className="justify-content-center">Edit Subcategory</h1>

            {
                message.length > 0 &&
                <Alert color="success" isOpen={true} toggle={() => onMessageDismiss()}>
                    {message}
                </Alert>
            }

            <FormGroup row className='mt-3' dark>
                <Label for="Category" sm={2}>Category</Label>
                <Col sm={10}>
                    <Input type="select" name="CategorySelect" id="Category" value={selectCateId} required onChange={e => onSelectCategory(e)}>
                        {
                            categories.length > 0 &&
                            categories.map(cate => (
                                <option key={cate.id} value={cate.id}>{cate.name}</option>
                            ))
                        }
                    </Input>
                </Col>
            </FormGroup>

            <FormGroup row className='mt-3' dark>
                <Label for="Subcategory" sm={2}>Subcategory</Label>
                <Col sm={10}>
                    <Input type="select" name="SubcategorySelect" id="Subcategory" value={selectSubcategoryId} required onChange={e => onSelectSubcategory(e)}>
                        {
                            subcategories.length > 0 &&
                            subcategories.map(subCate => (
                                <option key={subCate.id} value={subCate.id}>{subCate.name}</option>
                            ))
                        }
                    </Input>
                </Col>
            </FormGroup>

            {
                isLoading ?
                    (
                        <Container className="justify-content-center">
                            <Spinner type="grow" children='' className="justify-content-center" />
                        </Container>
                    ) : [
                        products.length > 0 ?
                            (
                                productTable()
                            ) : (
                                <Alert color="info" className="justify-content-center mt-3">
                                    <strong><i>product list empty!!</i></strong >
                                </Alert >
                            )
                    ]
            }
        </Container>
    )
}
