
select * from vschedule v join WeekWise w on v.schid=w.schid where SchCTId=3;

select * from vschedule v join WeekWise w on v.schid=w.schid where (v.offid=87 or v.offid=274) and w.CTId=2


select * from vschedule v join WeekWise w on v.schid=w.schid where w.CTId=2 and v.SchCTId=3


select v.schid,o.offid,o.courseid,o.secid,o.title,o.sectionname,v.teacherid,v.teachername,v.occupied,v.SchCTId,w.CTId from vschedule v 
join voffcourses o on v.offid=o.offid 
join Weekwise w on v.schid=w.schid 


select o.offid,o.courseid,o.secid,o.title,o.sectionname,v.teacherid,v.teachername,v.occupied,v.SchCTId,w.CTId from vschedule v 
join voffcourses o on v.offid=o.offid 
join Weekwise w on v.schid=w.schid
where v.SchCTId=null

select * from voffcourses;
select * from vschedule where SchCTId=3