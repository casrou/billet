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
                    loading: false
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
                        <td>{Math.floor(ticket.journeyTime / 60)}:{(ticket.journeyTime % 60)} ({ticket.journeyTime} min.)</td>
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
                <h1>Togbilletter 🚉</h1>
                <p>Find nemt de billigste togbilletter på en bestemt dag. Søgningen inkluderer DSB-Orange billetter og sommetider enkelte 'skjulte' billetter.</p>
                <p>Når du har fundet den perfekte billet:</p>
                <ul>
                    <li>Gå til <a href="https://www.dsb.dk/">dsb.dk</a> eller DSB-app'en på din mobil</li>
                    <li>Søg efter billetten med det præcise tidspunkt for afgang</li>
                    <li>Køb billetten 🙌</li>
                </ul>
                <form className="form-inline" onSubmit={this.handleSearch}>
                    <div className="form-group">
                        <label>
                        FRA:
                        </label>
                        <input placeholder="Aarhus" type="text" name="from" className="form-control" value={this.state.from} onChange={this.handleChange} />
                    </div>
                    <div className="form-group">
                        <label>
                        TIL:
                        </label>    
                        <input placeholder="Hurup" type="text" name="to" className="form-control" value={this.state.to} onChange={this.handleChange} />
                    </div>
                    <div className="form-group">
                        <label>
                        DATO:
                        </label>    
                        <input placeholder="03-08-2018" type="text" name="date" className="form-control" value={this.state.date} onChange={this.handleChange} />
                    </div>
                    <input className="btn btn-default" type="submit" value="Søg" />
                </form>
                <table className='table'>
                    <thead>
                        <tr>
                            <th>Afgang</th>
                            <th>Ankomst</th>
                            <th>Pris</th>
                            <th>Varighed</th>
                            <th>Skift</th>
                        </tr>
                    </thead>
                    {contents}
                </table>
            </div>
        );
    }
}
