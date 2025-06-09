/*
    Máxima Reutilização:
        Todos os métodos REST encapsulados no objeto rest
        Lógica de requisição centralizada, facilitando manutenções futuras

    Programação orientada a simplicidade:
        HTML limpo e semântico
        CSS básico mas funcional
        JavaScript modular e direto

    Componentes Nativos e código limpo:
        Uso da tag <dialog> para modais
        Event delegation para ações dinâmicas
        Formulários HTML5 com validação

    Workflow da aplicação:
        Listagem automática ao carregar
        CRUD completo com feedback visual
        Tratamento de erros básico
*/

import { rest } from './api.js';

class ClienteApp {
    constructor() {
        this.currentId = null;
        this.modal = document.getElementById('modalCliente');
        this.form = document.getElementById('formCliente');
        this.init();
    }

    init() {
        this.loadClientes();
        this.setupEvents();
    }

    async loadClientes() {
        try {
            const clientes = await rest.getClientes();
            this.renderClientes(clientes);
        } catch (error) {
            console.error('Erro ao carregar clientes:', error);
            alert('Falha ao carregar clientes');
        }
    }

    renderClientes(clientes) {
        const container = document.getElementById('listContainerClientes');
        container.innerHTML = '';

        if (clientes.length === 0) {
            container.innerHTML = '<p>Nenhum cliente cadastrado</p>';
            return;
        }

        const table = document.createElement('table');
        table.innerHTML =
            '<thead>'
            '    <tr>'
            '        <th>ID</th>'
            '        <th>Nome</th>'
            '        <th>Sexo</th>'
            '        <th>Endereço</th>'
            '        <th>Ações</th>'
            '    </tr>'
            '</thead>'
            '<tbody></tbody>';

        const tbody = table.querySelector('tbody');

        clientes.array.forEach(cliente => {
            const row = document.createElement('tr');
            row.innerHTML = 
            `<td>${cliente.id}</td>`
            `<td>${cliente.nome}</td>`
            `<td>${cliente.sexo}</td>`
            `<td>${cliente.endereco}</td>`
            `<td class="actions">`
            `   <button data-id="${cliente.id}" class=editBtn>Editar</button>`
            `   <button data-id="${cliente.id}" class=deleteBtn>Excluir</button>`
            `</td>`;

            tbody.appendChild(row);
        });

        container.appendChild(table);
    }

    setupEvents() {
        //Novo Cliente
        document.getElementById('newBtn').addEventListener('click', () => this.openModal());
        
        //Cancelar modal
        document.getElementById('closeTelefoneBtn').addEventListener('click', () => this.closeModal());

        //Salvar formulário
        this.form.addEventListener('submit', (e) => {
            e.preventDefault();
            this.SaveCliente();
        });

        //Delegation para botões de ação 
        document.addEventListener('click', (e) => {
            if (e.target.classList.contains('telefonesBtn')) {
                this.editTelefonesCliente(e.target.dataset.id);
            }
            
            if (e.target.classList.contains('editBtn')) {
                this.editCliente(e.target.dataset.id);
            }

            if (e.target.classList.contains('deleteBtn')) {
                this.deleteCliente(e.target.dataset.id);
            }
        });
    }

    async openModal(id = null) {
        this.currentId = id;
        const title = document.getElementById('modalTitle');

        if (id) {
            title.textContent = 'Editar Cliente';

            try {
                const cliente = await rest.getCliente(id);

                document.getElementById('nome').value = cliente.nome;
                document.getElementById('sexo').value = cliente.sexo;
                document.getElementById('sexo').value = cliente.endereco;
            } catch (error) {
                console.error('Erro ao carregar cliente: ', error);
                alert('Falha ao carregar cliente');
                return;
            }
        } else {
            title.textContent = 'Novo Cliente';
            this.form.reset();
        }

        this.modal.showModal();
    }

    closeModal() {
        this.modal.close();
    }

    async saveCliente() {
        const cliente = {
            nome: document.getElementById('nome').value,
            sexo: document.getElementById('sexo').value,
            endereco: document.getElementById('endereco').value
        };

        try {
            if (this.currentId) {
                await rest.updateCliente(this.currentId, cliente);
            } else {
                await rest.createCliente(cliente);
            }

            this.closeModal();
            this.loadClientes();
        } catch (error) {
            console.error('Erro ao salvar cliente: ', error);
            alert('Erro ao salvar cliente');
        }
    }

    async editCliente(id) {
        this.openModal(id);
    }

    async deleteCliente(id) {
        if (!confirm('tem certeza que deseja excluir este cliente?')) return;

        try {
            await rest.deleteCliente(id);
            this.loadClientes();
        } catch (error) {
            console.error('Erro ao excluir cliente: ', error);
            alert('Erro ao excluir cliente');
        }
    }
}

//Incia a aplicação
new ClienteApp();