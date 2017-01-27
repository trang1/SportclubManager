create or replace view v_adm_area as
select a.id, a.NAME, a.CODE, s.name subdivision, 
       e.position ass_emp_position, e.rank ass_emp_rank,
       c.lastname ass_emp_ln, c.firstname ass_emp_fn, c.middlename ass_emp_mn
from AREA a , subdivision s, areaassignment ass, employee e, citizen c
where s.id (+)= a.subdivision_id 
and ass.area_id (+)= a.id
and e.id (+)= ass.employee_id
and c.id (+)= e.citizen_id
order by a.code;
