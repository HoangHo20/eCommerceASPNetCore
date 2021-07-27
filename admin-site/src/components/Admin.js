import React, { Component } from 'react'
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import Navbar from './Navbar'
import CategoryCreate from './Category/CategoryCreate'
import CategoryModify from './Category/CategoryModify'
import CategoryModifyItem from './Category/CategoryModifyItem.js'
import SubcategoryCreate from './Subcategory/SubcategoryCreate.js'
import SubcategoryModify from './Subcategory/SubcategoryModify.js'
import SubcategoryModifyItem from './Subcategory/SubcategoryModifyItem.js'

export default class Admin extends Component {
    render() {
        return (
            <Router>
                <Navbar></Navbar>

                <Switch>
                    <Route exact path="/category/create">
                        <CategoryCreate />
                    </Route>
                    <Route exact path="/category/modify">
                        <CategoryModify />
                    </Route>
                    <Route exact path="/category/modify/:id">
                        <CategoryModifyItem />
                    </Route>
                    <Route exact path="/subcategory/create">
                        <SubcategoryCreate/>
                    </Route>
                    <Route exact path="/subcategory/modify/">
                        <SubcategoryModify />
                    </Route>
                    <Route exact path="/subcategory/modify/:id">
                        <SubcategoryModifyItem />
                    </Route>
                </Switch>
            </Router>
        )
    }
}
