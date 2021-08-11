import React, { Component } from 'react'
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Container } from 'reactstrap'

export default class Delete extends Component {
    constructor(props) {
        super(props);

        this.state = {
            assetCode: props.id,
            modal: props.isVisible
        }
    }

    toggle() {
        this.setState({
            modal: !this.state.modal
        })
    }

    onDeleteClick() {
        console.log(this.state.assetCode);

        this.toggle();
    }

    render() {
        return (
            <div>
                <Modal
                    isOpen={this.state.modal}
                    toggle={() => this.toggle()}
                    className={""}
                    centered
                    fade={false}
                    backdrop={false} >
                    <div className={"bg-light border"}>
                        <Container>
                            <Container className={"my-3 mx-4 text-danger"}>
                                <h5><strong>Are you sure?</strong></h5>
                            </Container>
                        </Container>
                    </div>
                    <ModalBody>
                        <Container className="mt-1 mb-3 mx-4">
                            Do you want to delete this asset?

                            <div className={"mt-4"}>
                                <Button color="danger" onClick={() => this.onDeleteClick()}>Delete</Button>

                                <Button color="light" className={"border mx-4"} onClick={() => this.toggle()}>Cancel</Button>
                            </div>
                        </Container>
                    </ModalBody>
                </Modal>
            </div>
        )
    }
}
