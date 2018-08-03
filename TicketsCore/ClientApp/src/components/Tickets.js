import React, { Component } from 'react';
import Moment from 'react-moment';

export class Tickets extends Component {
    displayName = Tickets.name

    constructor(props) {
        super(props);
        this.state = {
            tickets: [],
            loading: false,
            from: "",
            to: "",
            date: ""
        };
        
        this.handleChange = this.handleChange.bind(this);
        this.handleSearch = this.handleSearch.bind(this);
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value});
    }
    
    handleSearch(event) {
        this.setState({
            tickets: [],
            loading: true
        });
        
        fetch(`api/Tickets/GetTickets?from=${this.state.from}
            &to=${this.state.to}
            &date=${this.state.date}`)
            .then(response => response.json())
            .then(data => {
                this.setState({
                    tickets: data,
                    loading: false,
                    from: "",
                    to: "",
                    date: ""
                });
            });

        event.preventDefault();
    }

    static renderTicketsTable(tickets) {
        return (
            <tbody>
                {tickets.map(ticket =>
                    <tr key={ticket.arrivalDate}>
                        <td>
                            <Moment date={ticket.departureDate} format="DD/MM/YYYY HH:mm" interval={0}>
                            </Moment>
                        </td>
                        <td>
                            <Moment date={ticket.arrivalDate} format="DD/MM/YYYY HH:mm" interval={0}>
                            </Moment>
                        </td>
                        <td>{ticket.lowestPrice} kr.</td>
                        <td>{ticket.journeyTime} min.</td>
                        <td>{ticket.numberShifts}</td>
                    </tr>
                )}
            </tbody>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Tickets.renderTicketsTable(this.state.tickets);

        return (
            <div>
                <h1>Tickets</h1>
                <p>This component demonstrates fetching data from the server.</p>
                <form className="form-inline" onSubmit={this.handleSearch}>
                    <div className="form-group">
                        <label>
                        From:
                        </label>
                        <input placeholder="Aarhus" type="text" name="from" className="form-control" value={this.state.from} onChange={this.handleChange} />
                    </div>
                    <div className="form-group">
                        <label>
                        To:
                        </label>    
                        <input placeholder="Hurup" type="text" name="to" className="form-control" value={this.state.to} onChange={this.handleChange} />
                    </div>
                    <div className="form-group">
                        <label>
                        When:
                        </label>    
                        <input placeholder="03-08-2018" type="text" name="date" className="form-control" value={this.state.date} onChange={this.handleChange} />
                    </div>
                    <input className="btn btn-default" type="submit" value="Submit" />
                </form>
                <table className='table'>
                    <thead>
                        <tr>
                            <th>Departure</th>
                            <th>Arrival</th>
                            <th>Price</th>
                            <th>Duration</th>
                            <th>Changes</th>
                        </tr>
                    </thead>
                    {contents}
                </table>
            </div>
        );
    }
}
