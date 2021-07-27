import React, { Component } from 'react'
import { Link, Redirect } from 'react-router-dom'
import { Container, Table, Alert, Button, Spinner } from 'reactstrap'
import {
    aPost
} from '../../../utils/httpHelpers'
import {
    aGet
} from '../../../utils/httpHelpers'

export default class index extends Component {
    state = {
        categories: [],
        isLoading: true
    }

    popLoading(callback) {
        this.setState({
            isLoading: true
        }, () => {
            callback()
        })
    }

    getAllCategories() {
        aGet('Category')
            .then(response => {
                this.setState({
                    categories: response.data,
                    isLoading: false
                })
            })
            .catch((error) => {
                this.setState({
                    isLoading: false
                })
            })
    }

    componentDidMount() {
        this.getAllCategories()
    }

    onCategoryClick(id) {
        console.log('abc');

        return <Redirect
            to={{
                pathname: "/create",
            }}
        />
    }

    categoryTable() {
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
                        this.state.categories.map(category => (
                            <tr key={category.id} className="react-table-row">
                                <th scope="row">{category.id}</th>
                                <td>{category.name}</td>
                                <td>{category.description}</td>
                                <td>
                                    <Link to={`/category/modify/${category.id}`}>
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

    render() {
        return (
            <Container className='mt-5'>
                <h1 className="justify-content-center">Click a category to edit</h1>

                {
                    this.state.isLoading ?
                        (
                            <Container className="justify-content-center">
                                <Spinner type="grow" children='' className="justify-content-center" />
                            </Container>
                        ) : [
                            this.state.categories.length > 0 ?
                                (
                                    this.categoryTable()
                                ) : (
                                    <Alert color="info" className="justify-content-center mt-3">
                                        <strong><i>Category list empty!!</i></strong >
                                    </Alert >
                                )
                        ]
                }
            </Container>
        )
    }
}


