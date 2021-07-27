import React, { useState, Component } from 'react';
import {
    Collapse,
    Navbar,
    NavbarToggler,
    NavbarBrand,
    Nav,
    NavItem,
    NavLink,
    UncontrolledDropdown,
    DropdownToggle,
    DropdownMenu,
    DropdownItem,
    NavbarText,
    Container
} from 'reactstrap';

import {Link} from 'react-router-dom'

export default class index extends Component {
    state = {
        isOpen: false,
    };

    toggle() {
        this.setState({
            isOpen: !this.state.isOpen
        });
    };

    render() {
        return (
            <Navbar color="dark" dark expand="md">
                <Container>
                    <NavbarBrand href="/">Admin Site</NavbarBrand>
                    <NavbarToggler onClick={() => this.toggle()} />
                    <Collapse isOpen={this.state.isOpen} navbar>
                        <Nav className="mr-auto" navbar>
                            <UncontrolledDropdown nav inNavbar>
                                <DropdownToggle nav caret>
                                    Category
                                </DropdownToggle>
                                <DropdownMenu right>
                                    <DropdownItem href="/category/create/">
                                        Create
                                    </DropdownItem>
                                    <DropdownItem href="/category/modify/">
                                        Modify
                                    </DropdownItem>
                                </DropdownMenu>
                            </UncontrolledDropdown>
                        </Nav>
                        <Nav className="mr-auto" navbar>
                            <UncontrolledDropdown nav inNavbar>
                                <DropdownToggle nav caret>
                                    Subcategory
                                </DropdownToggle>
                                <DropdownMenu right>
                                    <DropdownItem href="/subcategory/create/">
                                        Create
                                    </DropdownItem>
                                    <DropdownItem href="/subcategory/modify/">
                                        Modify
                                    </DropdownItem>
                                </DropdownMenu>
                            </UncontrolledDropdown>
                        </Nav>
                        <Nav className="mr-auto" navbar>
                            <UncontrolledDropdown nav inNavbar>
                                <DropdownToggle nav caret>
                                    Product
                                </DropdownToggle>
                                <DropdownMenu right>
                                    <DropdownItem href="/product/create/">
                                        Create
                                    </DropdownItem>
                                    <DropdownItem href="/product/modify/">
                                        Modify
                                    </DropdownItem>
                                </DropdownMenu>
                            </UncontrolledDropdown>
                        </Nav>
                    </Collapse>
                </Container>
            </Navbar>

        )
    }
}


