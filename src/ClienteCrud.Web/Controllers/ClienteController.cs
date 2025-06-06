using ClienteCrud.Core.Model;
using ClienteCrud.Infra.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ClienteCrud.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteRepository _clienteRepository;
        private readonly TelefoneRepository _telefoneRepository;

        public ClienteController(ClienteRepository clienteRepository, TelefoneRepository telefoneRepository)
        {
            _clienteRepository = clienteRepository;
            _telefoneRepository = telefoneRepository;
        }

        [HttpGet] /* api/cliente */
        public ActionResult<IEnumerable<Cliente>> GetAll()
        {
            return Ok(_clienteRepository.GetAll());
        }

        [HttpGet("{id}")] /* api/cliente/1 */
        public ActionResult<Cliente> GetById(int id)
        {
            var cliente = _clienteRepository.GetById(id);
            if (cliente == null)
            {
                return NotFound();
            }

            return Ok(cliente);
        }

        [HttpPost] /* api/cliente */
        public ActionResult<Cliente> Create([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _clienteRepository.Save(cliente);
            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
        }

        [HttpPut("{id}")] /* api/cliente/1 */
        public ActionResult Update(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest();
            }

            var existingCliente = _clienteRepository.GetById(id);
            if (existingCliente == null)
            {
                return NotFound();
            }

            _clienteRepository.Save(cliente);
            return NoContent();
        }

        [HttpDelete("{id}")] /* api/cliente/1 */
        public ActionResult Delete(int id)
        {
            var cliente = _clienteRepository.GetById(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _clienteRepository.Delete(id);
            return NoContent();
        }

        [HttpGet("{id}/telefones")] /* api/cliente/1/Telefones */
        public ActionResult<IEnumerable<Telefone>> GetTelefones(int id)
        {
            var telefones = _clienteRepository.GetTelefoneByClienteId(id);
            return Ok(telefones);
        }
    }
}