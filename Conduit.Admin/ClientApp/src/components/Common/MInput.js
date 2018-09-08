import React, { Component } from 'react';
import PropTypes from 'prop-types';

class MInput extends Component {
    setTextValid(){
        return this.props.hasValid?"is-invalid form-control":"form-control"
    }
    render() {
        var hasValid=this.setTextValid();
        return (<div>
            <input type="text" onChange={this.props.onChange} name="text" data-validate="" value={this.props.value} className={hasValid} />
            <span className="invalid-feedback">Field is required</span>
        </div>);
    }
}
MInput.propTypes = {
    hasValid: PropTypes.bool.isRequired
  };
export default MInput;