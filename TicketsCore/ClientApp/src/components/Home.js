import React, { Component } from 'react';

export class Home extends Component {
  displayName = Home.name

  render() {
    return (
        <div>
            <br>
            <p>Built with:</p>
            <ul>
              <li><a href='https://get.asp.net/'>ASP.NET Core</a> and <a href='https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx'>C#</a> for cross-platform server-side code</li>
              <li><a href='https://facebook.github.io/react/'>React</a> for client-side code</li>
            </ul>
        </div>
    );
  }
}
