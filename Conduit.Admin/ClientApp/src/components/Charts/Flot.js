import $ from 'jquery';
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import deepEqual from 'deep-equal';

// Flot Charts
import 'flot';
import 'flot/jquery.flot.categories';
import 'flot/jquery.flot.pie';
import 'flot/jquery.flot.resize';
import 'flot/jquery.flot.time';
import 'jquery.flot.spline/jquery.flot.spline';
import 'jquery.flot.tooltip/js/jquery.flot.tooltip.min';

/**
 * Wrapper component for jquery-flot plugin
 */
class FlotChart extends Component {
    static propTypes = {
        /** data to display */
        data: PropTypes.array.isRequired,
        /** flot options object */
        options: PropTypes.object.isRequired,
        /** height of the container element */
        height: PropTypes.string,
        /** width of the container element */
        width: PropTypes.string
    }

    static defaultProps = {
        height: '300px',
        width: '100%'
    }

    componentWillMount() {
        if(typeof $.plot === 'undefined')
            throw new Error('Flot plugin not present.');
    }

    componentDidMount() {
        this.dreawChart();
    }

    componentWillReceiveProps(nextProps) {
        if (!deepEqual(nextProps.data, this.props.data) || !deepEqual(nextProps.options, this.props.options)) {
            this.dreawChart(nextProps);
        }
    }

    componentWillUnmount() {
        $(this.flotElement).data('plot').shutdown();
    }

    dreawChart(nextProps) {
        const data = (nextProps && nextProps.data) || this.props.data;
        const options = (nextProps && nextProps.options) || this.props.options;
        $.plot(this.flotElement, data, options);
    }

    setRef = node => {
        this.flotElement = node;
    }

    render() {
        const style = {
            height: this.props.height,
            width: this.props.width
        };

        return (
            <div ref={this.setRef} style={style} />
        );
    }
}

export default FlotChart;