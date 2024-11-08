import axios from "axios";

export const NetworkMethods = {
    GET: "GET",
    POST: "POST",
    PUT: "PUT",
    DELETE: "DELETE"
}

export default async function sendApiRequest({
                                                 method,
                                                 endpoint,
                                                 payload,
                                                 headers
                                             }) {
    const API_URL = process.env.REACT_APP_API_URL + endpoint;
    const requestConfig = {
        headers: {
            'x-public-key': process.env.REACT_APP_REQUEST_KEY,
            ...headers
        }
    };

    const finalObject = {
        status: 400,
        data: {},
        error: null,
        isSuccess: false
    };

    try {
        let response;

        switch (method) {
            case NetworkMethods.GET:
                response = await axios.get(API_URL, requestConfig);
                break;
            case NetworkMethods.POST:
                response = await axios.post(API_URL, payload, requestConfig);
                break;
            case NetworkMethods.PUT:
                response = await axios.put(API_URL, payload, requestConfig);
                break;
            case NetworkMethods.DELETE:
                response = await axios.delete(API_URL, requestConfig);
                break;
            default:
                throw new Error(`Invalid method: ${method}`);
        }

        if (response.status < 200 || response.status > 299)
        {
            finalObject.data = null;
            finalObject.status = 400;
            finalObject.error = response?.error ? response.error : response.statusText;
            finalObject.isSuccess = false;
        }
        else
        {
            finalObject.data = response.data;
            finalObject.status = 200;
            finalObject.isSuccess = true;
            finalObject.error = null;
        }
    } catch (error) {
        finalObject.data = null;
        finalObject.status = 400;
        finalObject.isSuccess = false;
        finalObject.error = error?.error ? error.error : error.response.data.error;
    }

    return finalObject;
}