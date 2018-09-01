
import React, { Component } from 'react';
import ContentWrapper from '../Layout/ContentWrapper';
import { Row, Col, Input, Card, CardHeader, CardBody, CardFooter, CustomInput } from 'reactstrap';
import { Route,Link } from 'react-router-dom';
import FormValidator from './../Forms/FormValidator';

/**
 * Validation flow using controlled components
 *
 * 1- User type on input
 * 2- onChange event trigger validation
 * 3- Validate methods are listed using "data-validate"
 *    attribute. Content must be an array in json format.
 * 4- The validation returns an object with format {[input name]: status}
 *    where status is an array of boolean per each method
 * 5- Methods that requires an argument, read the 'data-param' attribute
 * 6- Similarly, onSubmit event does a bulk validation on all form elements
 */

class AuthorItem extends Component {

    state = {
        /* Group each form state in an object.
           Property name MUST match the form name */
       
        formDemo: {
            text: '',
            email: '',
            number: '',
            integer: '',
            alphanum: '',
            url: '',
            password: '',
            password2: '',
            minlength: '',
            maxlength: '',
            length: '',
            minval: '',
            maxval: '',
            list: ''
        }
    }

     /**
      * Validate input using onChange event
      * @param  {String} formName The name of the form in the state object
      * @return {Function} a function used for the event
      */
    validateOnChange = event => {
        const input = event.target;
        const form = input.form
        const value = input.type === 'checkbox' ? input.checked : input.value;

        const result = FormValidator.validate(input);

        this.setState({
            [form.name]: {
                ...this.state[form.name],
                [input.name]: value,
                errors: {
                    ...this.state[form.name].errors,
                    [input.name]: result
                }
            }
        });

    }

    onSubmit = e => {
        const form = e.target;
        const inputs = [...form.elements].filter(i => ['INPUT', 'SELECT'].includes(i.nodeName))

        const { errors, hasError } = FormValidator.bulkValidate(inputs)

        this.setState({
            [form.name]: {
                ...this.state[form.name],
                errors
            }
        });

        console.log(hasError ? 'Form has errors. Check!' : 'Form Submitted!')

        e.preventDefault()
    }

    /* Simplify error check */
    hasError = (formName, inputName, method) => {
        return  this.state[formName] &&
                this.state[formName].errors &&
                this.state[formName].errors[inputName] &&
                this.state[formName].errors[inputName][method]
    }

    render() {
        return (
            <div>
                <div className="content-heading">
                    <div>CHI TIẾT TÁC GIẢ
                        <small>Vui lòng thêm nội dung bên dưới.</small>
                    </div>
                    <div className='ml-auto'>
                        <Link className="btn btn-secondary" to={`/authors`}>Quay lại</Link>
                    </div>
                </div>
               
                { /* START row */ }
                <Row>
                    <div className="col-md-12">
                        <form onSubmit={this.onSubmit} action="" name="formDemo">
                            { /* START card */ }
                            <Card className="card-default">
                                <CardHeader>
                                    <div className="card-title">Fields validation</div>
                                </CardHeader>
                                <CardBody>
                                    <legend className="mb-4">Type validation</legend>
                                    <fieldset>
                                        <div className="form-group row align-items-center">
                                            <label className="col-md-2 col-form-label">Required Text</label>
                                            <Col md={ 6 }>
                                                <Input type="text"
                                                    name="text"
                                                    invalid={this.hasError('formDemo','text','required')}
                                                    onChange={this.validateOnChange}
                                                    data-validate='["required"]'
                                                    value={this.state.formDemo.text}
                                                />
                                                <span className="invalid-feedback">Field is required</span>
                                            </Col>
                                            <Col md={ 4 }>
                                            </Col>
                                        </div>
                                    </fieldset>
                                    <fieldset>
                                        <div className="form-group row align-items-center">
                                            <label className="col-md-2 col-form-label">Email</label>
                                            <Col md={ 6 }>
                                                <Input type="email"
                                                    name="email"
                                                    invalid={this.hasError('formDemo','email','required')||this.hasError('formDemo','email','email')}
                                                    onChange={this.validateOnChange}
                                                    data-validate='["required", "email"]'
                                                    value={this.state.formDemo.email}/>
                                                { this.hasError('formDemo','email','required') && <span className="invalid-feedback">Field is required</span> }
                                                { this.hasError('formDemo','email','email') && <span className="invalid-feedback">Field must be valid email</span> }
                                            </Col>
                                            <Col md={ 4 }></Col>
                                        </div>
                                    </fieldset>
                                    <fieldset>
                                        <div className="form-group row align-items-center">
                                            <label className="col-md-2 col-form-label">Number</label>
                                            <Col md={ 6 }>
                                                <Input type="text"
                                                    name="number"
                                                    invalid={this.hasError('formDemo','number','number')}
                                                    onChange={this.validateOnChange}
                                                    data-validate='["number"]'
                                                    value={this.state.formDemo.number}/>
                                                <span className="invalid-feedback">Field must be valid number</span>
                                            </Col>
                                            <Col md={ 4 }>
                                            </Col>
                                        </div>
                                    </fieldset>
                                </CardBody>
                                <CardFooter className="text-center">
                                    <button type="submit" className="btn btn-info">Run validation</button>
                                </CardFooter>
                            </Card>
                            { /* END card */ }
                        </form>
                    </div>
                </Row>
                { /* END row */ }
            </div>
            );
    }

}

export default AuthorItem;

