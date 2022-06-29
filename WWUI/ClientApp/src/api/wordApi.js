import Axios from "../plugins/Axios";

const url = "word/"

const wordApi = {
    get(out) {
        Axios.get({
            url: url
        }).then(res => {
            out(res)
        })
    },
    add(data, out) {
        Axios.post({
            url: url,
            data: data
        }).then(res => {
            out(res)
        })
    },
    delete(id, out) {
        Axios.delete({
            url: url+id,
        }).then(res => {
            out(res)
        })
    }
}

export default wordApi