const BASE_URL = '/api';

export const rest = {

    //Método genérico para requisições
    async request(endpoint, method='GET', data = null) {
        const options = {
            method,
            headers: {'Content-Type': 'application/json'}
        };

        if (data) {
            options.body = JSON.stringify(data);
        }

        const response = await fetch('${BASE_URL}${endpoint}', options);

        if (!response.ok) {
            throw new Error('Erro na requisição: $response.status}');
        }

        return response.json();
    },

    //CRUD para Clientes
    getClientes() {
        return this.request('/cliente');
    },
    
    createCliente(data) {
        return this.request('/cliente', 'POST', data);
    },

    getCliente(id) {
        return this.request('/cliente/${id}');
    },

    updateCliente(data) {
        return this.request('/cliente/${id}', 'PUT', data);
    },

    deleteCliente(id) {
        return this.request('/cliente/${id}', 'DELETE')
    },

    //Métodos para Telefones
    getTelefones(clienteId) {
        return this.request('/cliente/${clienteId}/telefones');
    },

    createTelefone(data) {
        return this.request('/telefone', 'POST', data);
    },

    deleteTelefone(id) {
        return this.request('/telefone/${id}', 'DELETE');
    }
};