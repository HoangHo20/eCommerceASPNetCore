import React, { useEffect, useState } from 'react'
import { Link, Redirect, useParams } from 'react-router-dom'
import { Container, Table, Alert, Button, Spinner, FormGroup, Label, Col, Input } from 'reactstrap'
import { aGet } from '../../utils/httpHelpers';

export default function SubcategoryModify() {
    const [isLoading, setIsLoading] = useState(false);
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [message, setMessage] = useState('');
    const [categories, setCategories] = useState([]);
    const [selectCateId, setSelectCateId] = useState(-1);
    const [subcategories, setSubcategories] = useState([]);

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
                    setSelectCateId(1);
                    getSubcategoryByCategoryId(1);
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
            setSubcategories(response.data);
        })
        .catch(error => {
            setMessage('Get subcategories list error');
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

    function subcategoryTable() {
        return (
            <Table hover className="mt-3 react-table">
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Name</th>
                        <th>Description</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {
                        subcategories.map(subCate => (
                            <tr key={subCate.id} className="react-table-row">
                                <th scope="row">{subCate.id}</th>
                                <td>{subCate.name}</td>
                                <td>{subCate.description}</td>
                                <td>
                                    <Link to={`/subcategory/modify/${subCate.id}`}>
                                        <Button color='info' className="react-table-row-action" >Edit</Button>
                                    </Link>
                                    <Button color='danger' className="react-table-row-action">Delete</Button>
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
                    <Input type="select" name="select" id="Category" value={selectCateId} required onChange={e => onSelectCategory(e)}>
                        {
                            categories.length > 0 &&
                            categories.map(cate => (
                                <option key={cate.id} value={cate.id}>{cate.name}</option>
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
                        subcategories.length > 0 ?
                            (
                                subcategoryTable()
                            ) : (
                                <Alert color="info" className="justify-content-center mt-3">
                                    <strong><i>Subcategory list empty!!</i></strong >
                                </Alert >
                            )
                    ]
            }
        </Container>
    )
}
