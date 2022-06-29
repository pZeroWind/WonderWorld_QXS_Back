import Axios from "../plugins/Axios";

const url = "book/"

const bookApi = {
    type(out) {
        Axios.get({
            url:url+"type"
        }).then(res => {
            out(res.data)
        })
    },
    banner(out) {
        Axios.get({
            url:url+"banner"
        }).then(res => {
            out(res.data)
        })
    },
    changeBanner(data,out) {
        Axios.put({
            url: url + "banner",
            data:data
        }).then(res => {
            out(res.data)
        })
    },
    getList(data,out) {
        Axios.get({
            url: url + "getList",
            params:data
        }).then(res => {
            out(res)
        })
    },
    state(out) {
        Axios.get({
            url: url + "state"
        }).then(res => {
            out(res.data)
        })
    },
    ban(id, out) {
        Axios.get({
            url:url+"ban/"+id
        }).then(res => {
            out(res)
        })
    }
}

export default bookApi