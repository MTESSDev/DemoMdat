# ObtenirReservationParIdTest

## 1 - Calcule avec des nuit�es � haut tarif

Description

``````yaml
mockAppelTableSQLReservation:
  Value: 
    Id: 231
    NumeroLot: 23A
    DateDebut: 2023-06-30
    DateFin: 2023-07-06

expected:
  verify: 
    - type: match
      allowAdditionalProperties: true
      data:
        NombreNuitees: 6
        NombreNuiteesHautTarif: 5
``````


## 2 - Calcule sans nuit�e � haut tarif

Description

``````yaml
mockAppelTableSQLReservation:
  Value: 
    Id: 231
    NumeroLot: 23A
    DateDebut: 2023-06-05
    DateFin: 2023-06-08

expected:
  verify: 
    - type: match
      allowAdditionalProperties: true
      data:
        NombreNuitees: 6
        NombreNuiteesHautTarif: 5
``````