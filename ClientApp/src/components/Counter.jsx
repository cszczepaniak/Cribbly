import * as React from 'react';
import { connect } from 'react-redux';
import * as CounterStore from '../store/Counter';

class Counter extends React.PureComponent {
  render() {
    return (
      <React.Fragment>
        <h1>Counter</h1>

        <p>This is a simple example of a React component.</p>

        <p aria-live='polite'>
          Current count: <strong>{this.props.count}</strong>
        </p>

        <button
          type='button'
          className='btn btn-primary btn-lg'
          onClick={() => {
            this.props.increment();
          }}
        >
          Increment
        </button>
      </React.Fragment>
    );
  }
}

export default connect(
  state => state.counter,
  CounterStore.actionCreators
)(Counter);
