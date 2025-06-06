using ClienteCrud.Core.Model;
using ClienteCrud.Infra.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ClienteCrud.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TelefoneController : ControllerBase
    {
        private readonly TelefoneRepository _telefoneRepository;

        public TelefoneController(TelefoneRepository telefoneRepository) => _telefoneRepository = telefoneRepository;

        [HttpGet("{id}")] /* api/telefone/1 */
        public ActionResult GetById(int id)
        {
            var telefone = _telefoneRepository.GetById(id);
            if (telefone == null)
            {
                return NotFound();
            }

            return Ok(telefone);
        }

        [HttpPost] /* api/telefone */
        public ActionResult<Telefone> Create([FromBody] Telefone telefone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _telefoneRepository.Save(telefone);
            return CreatedAtAction(nameof(GetById), new { id = telefone.Id }, telefone);
        }

        [HttpPut("{id}")] /* api/telefone/1 */
        public ActionResult Update(int id, [FromBody] Telefone telefone)
        {
            if (id != telefone.Id)
            {
                return BadRequest();
            }

            var existingTelefone = _telefoneRepository.GetById(id);
            if (existingTelefone == null)
            {
                return NotFound();
            }

            _telefoneRepository.Save(telefone);
            return NoContent();
        }

        [HttpDelete("{id}")] /* api/telefone/1 */
        public ActionResult Delete(int id)
        {
            var telefone = _telefoneRepository.GetById(id);
            if (telefone == null)
            {
                return NotFound();
            }

            _telefoneRepository.Delete(telefone);
            return NoContent();
        }

    }
}