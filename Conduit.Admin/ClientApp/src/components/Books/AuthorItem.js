
import React, { Component } from 'react';
import ContentWrapper from '../Layout/ContentWrapper';
import { Row, Col, Card, CardHeader, CardBody, CardFooter, CustomInput } from 'reactstrap';
import { Route, Link } from 'react-router-dom';
import FormValidator from './../Forms/FormValidator';

import Form from 'react-validation/build/form';
import Input from 'react-validation/build/input';
import CheckButton from 'react-validation/build/button';
// import Select from 'react-validation/build/select';
import { isEmail, isEmpty } from 'validator';
//import Select from 'react-select';
//import 'react-select/dist/react-select.css'
import MInput from './../Common/MInput';
import MSelect from './../Common/MSelect';
import MDate from './../Common/MDate';

const required = (value) => {
    if (isEmpty(value)) {
        return <small className="form-text text-danger">This field is required</small>;
    }
}

const email = (value) => {
    if (!isEmail(value)) {
        return <small className="form-text text-danger">Invalid email format</small>;
    }
}

const minLength = (value) => {
    if (value.trim().length < 6) {
        return <small className="form-text text-danger">Password must be at least 6 characters long</small>;
    }
}
const STATES = [
    { value: 'australian-capital-territory', label: 'Australian Capital Territory', className: 'State-ACT' },
    { value: 'new-south-wales', label: 'New South Wales', className: 'State-NSW' },
    { value: 'victoria', label: 'Victoria', className: 'State-Vic' },
    { value: 'queensland', label: 'Queensland', className: 'State-Qld' },
    { value: 'western-australia', label: 'Western Australia', className: 'State-WA' },
    { value: 'south-australia', label: 'South Australia', className: 'State-SA' },
    { value: 'tasmania', label: 'Tasmania', className: 'State-Tas' },
    { value: 'northern-territory', label: 'Northern Territory', className: 'State-NT' },
]

class AuthorItem extends Component {

    constructor(props){
        super(props);

        this.state = {
            selectValue: 0,
            minput:""
        }
    }

    onSubmit = e => {

        e.preventDefault();
        this.form.validateAll();

        if (this.checkBtn.context._errors.length === 0) {
            alert('success')
        }
        else {
            alert('faild')
        }
    }

    validDate(currentDate, selectedDate) {
        console.log(currentDate, selectedDate);
        return true;
    }
    handleChange(e) {
       
        this.setState({ selectValue: e.value });
    }
    onChangeMInput(e){
        this.setState({ minput: e.target.value });
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

                { /* START row */}
                <Row>
                    <div className="col-md-12">
                        <Form onSubmit={e => this.onSubmit(e)} ref={c => { this.form = c }}>
                            { /* START card */}
                            <Card className="card-default">

                                <CardBody>

                                    <div className="container">
                                        <div className="login-container">

                                            <div className="form-box">
                                                <MInput value={this.state.minput} 
                                                    hasValid={true}  
                                                    onChange={this.onChangeMInput.bind(this)}/>
                                                <MSelect
                                                    name="select-name"
                                                    hasValid={true}  
                                                    value={this.state.selectValue}
                                                    onChange={this.handleChange.bind(this)}
                                                    options={STATES}
                                                />
                                                <MDate dateFormat="DD-MM-YYYY" 
                                                    closeOnSelect={true} isValidDate={this.validDate.bind(this)}
                                                    timeFormat={false} inputProps={{ className: 'form-control' }} />
                                                <Input
                                                    name="email"
                                                    onChange={this.onChangeHandler}
                                                    type="text"
                                                    placeholder="Email"
                                                    className="form-control"
                                                    validations={[required, email]}
                                                />
                                                <Input
                                                    name="password"
                                                    onChange={this.onChangeHandler}
                                                    type="password"
                                                    placeholder="Password"
                                                    className="form-control"
                                                    validations={[required, minLength]}
                                                />


                                            </div>
                                        </div>
                                    </div>

                                </CardBody>
                                <CardFooter className="text-center">
                                    <button type="submit" className="btn btn-info">Lưu</button>
                                    <CheckButton style={{ display: 'none' }} ref={c => { this.checkBtn = c }} />
                                </CardFooter>

                            </Card>
                            { /* END card */}
                        </Form>
                    </div>
                </Row>
                { /* END row */}
            </div>
        );
    }

}

export default AuthorItem;

