import * as React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import * as WeatherForecastsStore from '../store/WeatherForecasts';

class FetchData extends React.PureComponent {
  // This method is called when the component is first added to the document
  componentDidMount() {
    this.ensureDataFetched();
  }

  // This method is called when the route parameters change
  componentDidUpdate() {
    this.ensureDataFetched();
  }

  render() {
    return (
      <React.Fragment>
        <h1 id='tabelLabel'>Weather forecast</h1>
        <p>
          This component demonstrates fetching data from the server and working
          with URL parameters.
        </p>
        {this.renderForecastsTable()}
        {this.renderPagination()}
      </React.Fragment>
    );
  }

  ensureDataFetched() {
    const startDateIndex =
      parseInt(this.props.match.params.startDateIndex, 10) || 0;
    this.props.requestWeatherForecasts(startDateIndex);
  }

  renderForecastsTable() {
    return (
      <table className='table table-striped' aria-labelledby='tabelLabel'>
        <thead>
          <tr>
            <th>Date</th>
            <th>Temp. (C)</th>
            <th>Temp. (F)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
          {this.props.forecasts.map(forecast => (
            <tr key={forecast.date}>
              <td>{forecast.date}</td>
              <td>{forecast.temperatureC}</td>
              <td>{forecast.temperatureF}</td>
              <td>{forecast.summary}</td>
            </tr>
          ))}
        </tbody>
      </table>
    );
  }

  renderPagination() {
    const prevStartDateIndex = (this.props.startDateIndex || 0) - 5;
    const nextStartDateIndex = (this.props.startDateIndex || 0) + 5;

    return (
      <div className='d-flex justify-content-between'>
        <Link
          className='btn btn-outline-secondary btn-sm'
          to={`/fetch-data/${prevStartDateIndex}`}
        >
          Previous
        </Link>
        {this.props.isLoading && <span>Loading...</span>}
        <Link
          className='btn btn-outline-secondary btn-sm'
          to={`/fetch-data/${nextStartDateIndex}`}
        >
          Next
        </Link>
      </div>
    );
  }
}

export default connect(
  state => state.weatherForecasts, // Selects which state properties are merged into the component's props
  WeatherForecastsStore.actionCreators // Selects which action creators are merged into the component's props
)(FetchData);
