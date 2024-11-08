import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import ErrorBoundary from "./routes/error/error.route";
import {Toaster} from "react-hot-toast";
import {ThemeProvider} from "@mui/material";
import {PersistGate} from "redux-persist/integration/react";
import {persistor, store} from "./store/store";
import {Provider} from "react-redux";

import theme from "./lib/Theme";

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
    <Provider store={store}>
        <PersistGate persistor={persistor}>
            <ThemeProvider theme={theme}>
                <ErrorBoundary>
                    <App/>
                    <Toaster/>
                </ErrorBoundary>
            </ThemeProvider>
        </PersistGate>
    </Provider>
);
