import Select from 'react-select';
import 'react-select/dist/react-select.css';
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';
{/*Ngoai styled-components co the dung emotion*/ }
{/*https://www.styled-components.com/docs/basics*/ }
const SelectStyle = styled(Select)`
	&.Select  {
        
        .Select-control {
           
                border:  ${props => props.isvalid ? "1px solid red" : "none"};
		}
      
		
	}

	& .Select-placeholder {
		font-size: smaller;
	}
`
const Button = styled.button`
  /* Adapt the colours based on primary prop */
  background: ${props => props.primary ? "palevioletred" : "white"};
  color: ${props => props.primary ? "white" : "palevioletred"};

  font-size: 1em;
  margin: 1em;
  padding: 0.25em 1em;
  border: 2px solid palevioletred;
  border-radius: 3px;
`;


class MSelect extends Component {
    setTextValid() {
        return this.props.hasValid ? "is-invalid" : ""
    }
    render() {
        var hasValid = this.setTextValid();
        return (
            <div>
                <Button>Normal Button</Button>
                <Button primary>Primary Button</Button>
                <SelectStyle isvalid={true}
                    name="select-name" data-validate="" className={hasValid}
                    value={this.props.value}
                    onChange={this.props.onChange}
                    options={this.props.options}
                />
                <span className="invalid-feedback">Field is required</span>
            </div>
        );
    }
}
MSelect.propTypes = {
    hasValid: PropTypes.bool.isRequired
};
export default MSelect;
// import React from "react";
// import ReactSelect from "react-select";
// import styled from 'styled-components';
// //Ngoai styled-components co the dung emotion
// const MSelect = styled(ReactSelect)`
// 	&.Select  {

// 		.Select-control {
// 			border: 1px solid red;
//             border-radius: 4px;
// 		}
// 	}

// 	& .Select-placeholder {
// 		font-size: smaller;
// 	}
// `

// export default (props) => <MSelect multi {...props} />