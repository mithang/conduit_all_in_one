import Datetime from 'react-datetime';
import 'react-datetime/css/react-datetime.css';
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';

class MDate extends Component {
    setTextValid() {
        return this.props.hasValid ? "is-invalid" : ""
    }
    render() {
        var hasValid = this.setTextValid();
        return (
            <div>
                <Datetime dateFormat={this.props.dateFormat}
                    closeOnSelect={true} isValidDate={this.props.isValidDate}
                    timeFormat={false} inputProps={{ className: 'form-control' }} />
                <span className="invalid-feedback">Field is required</span>
            </div>
        );
    }
}
MDate.propTypes = {
    hasValid: PropTypes.bool.isRequired
};
export default MDate;
