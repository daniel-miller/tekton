# Contact / Location / Data

Location is a feature set in the Contact application component.
  
Classes in this Data folder implement data access for this feature set. This is the persistence layer for Location.

## Future Schema Changes

When time and opportunity permit, the following database schema changes should be considered, to improve alignment with current database naming conventions:

* Move table `t_country` from schema `location` to schema `contact`.
* Rename table from `t_country` to ``.
* Move table `t_province` from schema `location` to schema `contact`.
* Rename table from `t_province` to ``.
