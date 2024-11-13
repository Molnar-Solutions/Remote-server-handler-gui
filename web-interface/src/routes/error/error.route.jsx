import "./error.css"

import React, {Component, ErrorInfo} from 'react';
import NotFoundImage from "../../assets/computer-156951_640.png";
import {useNavigate} from "react-router-dom";

class ErrorBoundary extends Component {
    constructor(props) {
        super(props);
        this.state = {hasError: false};
    }

    componentDidCatch(error, errorInfo) {
        console.error("error caught")
        console.error("error", error)
        console.error("error info ", errorInfo)
        this.setState({hasError: true});
    }

    resetErrorBoundry = () => {
        this.setState((pre) => ({
            ...pre,
            hasError: !pre.hasError
        }))
    }

    render() {
        if (this.state.hasError) {
            return <ErrorTextComponent resetErrorBoundry={this.resetErrorBoundry}/>
        }

        return this.props.children;
    }
}

function ErrorTextComponent({resetErrorBoundry}) {
    const navigate = useNavigate();
    return <div className='error-page'>
        <img src={NotFoundImage} alt='Page not found' style={{ maxWidth: "250px", height: "auto" }}/>
        <p className='error-msg'>
            Whoops! Something went wrong!
            <button className='btn' onClick={() => {
                try {
                    resetErrorBoundry();
                } catch (e) {
                }
                navigate("/");
            }}>
                Refresh the page
            </button>
        </p>
    </div>
}

export default ErrorBoundary;