import axios from "axios";

const endpoint = process.env.REACT_APP_ENDPOINT;
const token = localStorage.getItem("token");
const requestConfig = {
    headers: {
    "Content-Type": "multipart/form-data"
  }
};

function aGet(url) {
    console.log(token);
    console.log();
    return axios.get(endpoint + url);
}

function aPut(url, body) {
    return axios.put(endpoint + url, body, requestConfig);
}

function aPost(url, body) {
    return axios.post(endpoint + url, body, requestConfig);
}

function aDelete(url) {
    return axios.delete(endpoint + url);
}


export {
    aGet,
    aPut,
    aPost,
    aDelete
}